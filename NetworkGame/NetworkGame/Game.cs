using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using NetworkGame.Content;
using NetworkGame.Engine;
using NetworkGame.Logic;
using NetworkGame.Menus;
using NetworkGame.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkGame
{
    public class Game
    {
        internal static OpeningGameContent OpeningContent { get; private set; }
        internal static GameContent Content { get; private set; }

        Task m_initContentTask;
        InputWorker m_input;
        TickTimer m_timer;
        DefaultCanvas m_screenOpeningCanvas;
        DefaultCanvas m_screenCanvas;
        Profile m_profile;

        bool m_opening;

        SignalBackground m_background;
        MainMenu m_mainMenu;
        SelectLevelMenu m_selectLevelMenu;
        LoadingScreen m_loadingScreen;
        Field m_field;
        InGameMenu m_inGameMenu;
        LevelCompleteMenu m_levelCompleteMenu;

        ScreenManager m_screenManager;

        TextSpriteWithShadow m_fpsCounter;

        List<BaseScreen> m_touchTargets;
        List<BaseScreen> m_mouseTargets;

        List<IUpdatable> m_updateTargets;
        List<Engine.IDrawable> m_drawTargets;

        public ContentManager ContentManager { get; protected set; }
        public GraphicsDevice Device { get; protected set; }
        public GameParams Params { get; protected set; }

        public Game(GraphicsDevice device, ContentManager contentManager)
        {
            this.Device = device;
            this.ContentManager = contentManager;
        }

        public void Init(GameParams _params)
        {
            Params = _params;

            InitOpening();
            m_initContentTask = InitContent();
        }

        void InitOpening()
        {
            OpeningContent = new OpeningGameContent(ContentManager, Params);

            m_screenOpeningCanvas = new DefaultCanvas(Device, false);
            m_screenOpeningCanvas.View.SetColor(OpeningContent.BackgroundColor);

            m_opening = true;
        }

        async Task InitContent()
        {
            var loadingStopwatch = new Stopwatch();
            loadingStopwatch.Start();

            await Task.Run(() => InitContentCore());

            const int maxLoadMilliseconds = 150;

            loadingStopwatch.Stop();
            if (loadingStopwatch.ElapsedMilliseconds >= maxLoadMilliseconds)
                m_screenManager.FirsScreenAppear(m_mainMenu);
            else
                m_timer.Add(maxLoadMilliseconds - loadingStopwatch.ElapsedMilliseconds, () => m_screenManager.FirsScreenAppear(m_mainMenu));

            m_opening = false;
        }

        void InitContentCore()
        {
            Content = new GameContent(Device, OpeningContent, Params);

            m_input = new InputWorker();
            m_timer = new TickTimer();

            m_profile = new Profile(Params.ProfileDir);
            m_profile.LoadProfileData();

            m_screenManager = new ScreenManager();

            m_screenCanvas = new DefaultCanvas(Device, false);
            m_screenCanvas.View.SetColor(OpeningContent.BackgroundColor);

            m_background = new SignalBackground();
            m_background.AddToCanvas(m_screenCanvas);

            m_loadingScreen = new LoadingScreen(Device) { Visible = false };
            m_loadingScreen.AddToCanvas(m_screenCanvas);

            m_field = new Field(Device) { Visible = false };
            m_field.VisibleChanged = ((bool fieldVisible) => m_background.Visible = !fieldVisible);
            m_field.MenuButton.Click = (() => m_screenManager.ModalScreenFront(m_inGameMenu, m_field));
            m_field.Complete = DelayedLevelComplete;
            m_field.AddToCanvas(m_screenCanvas);

            if (Params.ShowFps)
            {
                m_fpsCounter = new TextSpriteWithShadow(Content.Fonts.FieldStatusFont);
                m_screenCanvas.Add(m_fpsCounter);
            }

            PrepareMainMenu();
            PrepareSelectLevelMenu();
            PrepareInGameMenu();
            PrepareLevelCompleteMenu();

            PrepareInput();

            PrepareComponents();
        }

        void PrepareMainMenu()
        {
            m_mainMenu = new MainMenu(Device, m_profile) { IsProcessInput = false, Visible = false };
            m_mainMenu.PlayOrContinueButton.Click = (() => m_screenManager.ScreenFront(m_loadingScreen, m_mainMenu,
                async () => await LoadFieldDataAndShowField(m_profile.Levels.Last(), false)));
            m_mainMenu.LevelSelectButton.Click = (() => m_screenManager.ScreenFront(m_selectLevelMenu, m_mainMenu));
            if (m_mainMenu.ExitButton != null)
                m_mainMenu.ExitButton.Click = (() => DelayedExit(m_mainMenu));
            m_mainMenu.AddToCanvas(m_screenCanvas);
        }

        void PrepareSelectLevelMenu()
        {
            m_selectLevelMenu = new SelectLevelMenu(Device) { Visible = false };
            m_selectLevelMenu.LevelsListView.AddItems(m_profile.Levels.Select(x => new LevelListViewItem(x)));
            m_selectLevelMenu.LevelsListView.ItemClick = ((LevelListViewItem item) => m_screenManager.ScreenFront(m_loadingScreen, m_selectLevelMenu,
                async () => await LoadFieldDataAndShowField(item.Descriptor, false)));
            m_selectLevelMenu.BackButton.Click = (() => m_screenManager.ScreenBack(m_mainMenu, m_selectLevelMenu));
            m_selectLevelMenu.AddToCanvas(m_screenCanvas);
        }

        void PrepareInGameMenu()
        {
            m_inGameMenu = new InGameMenu(Device) { Visible = false };
            m_inGameMenu.RestartButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                async () => await LoadFieldDataAndShowField(m_field.Descriptor, true), m_inGameMenu, m_field));
            m_inGameMenu.MainMenuButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                async () => await SaveFieldDataAndGoToScreen(m_mainMenu), m_inGameMenu, m_field));
            m_inGameMenu.LevelSelectButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                async () => await SaveFieldDataAndGoToScreen(m_selectLevelMenu), m_inGameMenu, m_field));
            if (m_inGameMenu.ExitButton != null)
            {
                m_inGameMenu.ExitButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                    async () => await SaveFieldDataAndExit(), m_inGameMenu, m_field));
            }
            m_inGameMenu.BackButton.Click = (() => m_screenManager.ModalScreenBack(m_field, m_inGameMenu));
            m_inGameMenu.AddToCanvas(m_screenCanvas);
        }

        void PrepareLevelCompleteMenu()
        {
            m_levelCompleteMenu = new LevelCompleteMenu(Device) { Visible = false };
            m_levelCompleteMenu.NextButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                async () => await LoadFieldDataAndShowField(m_profile.Levels[m_field.Descriptor.Index + 1], false), m_levelCompleteMenu, m_field));
            m_levelCompleteMenu.RestartButton.Click = (() => m_screenManager.ModalScreenOut(m_loadingScreen,
                async () => await LoadFieldDataAndShowField(m_field.Descriptor, true), m_levelCompleteMenu, m_field));
            m_levelCompleteMenu.LevelSelectButton.Click = (() => m_screenManager.ModalScreenOut(m_selectLevelMenu, null, m_levelCompleteMenu, m_field));
            m_levelCompleteMenu.MainMenuButton.Click = (() => m_screenManager.ModalScreenOut(m_mainMenu, null, m_levelCompleteMenu, m_field));
            if (m_levelCompleteMenu.ExitButton != null)
                m_levelCompleteMenu.ExitButton.Click = (() => DelayedExit(m_levelCompleteMenu));
            m_levelCompleteMenu.AddToCanvas(m_screenCanvas);
        }

        void PrepareInput()
        {
            m_touchTargets = new List<BaseScreen>()
            {
                m_mainMenu,
                m_selectLevelMenu,
                m_field,
                m_inGameMenu,
                m_levelCompleteMenu,
            };

            m_mouseTargets = new List<BaseScreen>()
            {
                m_mainMenu,
                m_selectLevelMenu,
                m_field,
                m_inGameMenu,
                m_levelCompleteMenu,
            };
        }

        void PrepareComponents()
        {
            m_updateTargets = new List<IUpdatable>()
            {
                m_background,
                m_selectLevelMenu,
                m_field,
                m_timer,
                AnimationManager.DefaultManager,
            };

            m_drawTargets = new List<Engine.IDrawable>()
            {
                m_mainMenu,
                m_selectLevelMenu,
                m_field,
                m_inGameMenu,
                m_levelCompleteMenu,
                m_screenCanvas,
            };
        }

        public void SaveDataIfNeed()
        {
            if (m_opening)
                return;

            if (m_screenManager.ActiveScreen == null)
                return;

            //just save field if need
            if ((m_screenManager.ActiveScreen == m_field || m_screenManager.ActiveScreen == m_inGameMenu) && m_field.Visible)
                m_profile.SaveFieldData(m_field.GetFieldData(), m_field.Descriptor);
        }

        public void Update(GameTime gameTime)
        {
            if (m_opening)
                return;

            if (Params.TouchInput)
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    var touchParams = m_input.GetTouchInput();
                    foreach (var touchTarget in m_touchTargets)
                        touchTarget.Touch(ref touchParams);
                }
            }

            if (Params.GamePadInput)
            {
                var gamePadParams = m_input.GetGamePadInput();
                if (gamePadParams.BackButtonPressed)
                    Back();
            }

            if (Params.MouseInput)
            {
                var mouseParams = m_input.GetMouseInput();
                if (mouseParams.IsInput)
                {
                    foreach (var mouseTarget in m_mouseTargets)
                        mouseTarget.Mouse(mouseParams);
                }
            }

            if (Params.KeyboardInput)
            {
                var keyboardParams = m_input.GetKeyboardInput();
                if (keyboardParams.PressedKeysSet.Contains(Microsoft.Xna.Framework.Input.Keys.Escape))
                    Back();
            }

            foreach (var updateTarget in m_updateTargets)
                updateTarget.Update(gameTime);
        }

        void UpdateFpsCounter(GameTime gameTime)
        {
            if (!Params.ShowFps)
                return;

            m_fpsCounter.Text = (1000 / gameTime.ElapsedGameTime.TotalMilliseconds).ToString("N1") + " FPS";

            int fpsCounterOffset = Content.GetSizeInDpi(5);
            m_fpsCounter.X = Device.Viewport.Width - m_fpsCounter.Width / 2 - fpsCounterOffset;
            m_fpsCounter.Y = m_fpsCounter.Height / 2 + fpsCounterOffset - m_fpsCounter.Font.BaselineOffset;
        }

        public void Draw(GameTime gameTime)
        {
            if (m_opening)
            {
                m_screenOpeningCanvas.Draw(gameTime);
            }
            else
            {
                foreach (var drawTarget in m_drawTargets)
                    drawTarget.Draw(gameTime);

                if (Params.ShowFps)
                    UpdateFpsCounter(gameTime);
            }
        }

        void Back()
        {
            if (m_opening)
                return;

            m_screenManager.ActiveScreen?.Back();
        }

        async Task LoadFieldDataAndShowField(LevelDescriptor descriptor, bool isGenerateNew)
        {
            m_field.BeginUpdateFieldData();
            await Task.WhenAll(
                Task.Delay(300),
                Task.Run(() =>
                {
                    if (isGenerateNew)
                        m_profile.DeleteFieldData(descriptor);
                    m_field.UpdateFieldData(m_profile.GetFieldData(descriptor), descriptor);
                }));
            m_field.EndUpdateFieldData();

            m_screenManager.ScreenFront(m_field, m_loadingScreen);
        }

        void DelayedLevelComplete(CompleteBundle bundle)
        {
            m_field.IsProcessInput = false;

            Task.Run(() =>
            {
                if (bundle.Descriptor.Index == m_profile.Levels.Last().Index)
                {
                    m_profile.AddLevel();
                    m_selectLevelMenu.LevelsListView.AddItem(new LevelListViewItem(m_profile.Levels.Last()));
                    m_profile.SaveProfileData();
                }
                m_profile.DeleteFieldData(bundle.Descriptor);
            });

            m_levelCompleteMenu.SetCompleteInfo(bundle);
            m_timer.Add(1000, () => m_screenManager.ModalScreenFront(m_levelCompleteMenu, m_field));
        }

        async Task SaveFieldDataAndGoToScreen(BaseScreen toScreen)
        {
            m_field.BeginUpdateFieldData();
            await Task.WhenAll(
                Task.Delay(200),
                Task.Run(() => m_profile.SaveFieldData(m_field.GetFieldData(), m_field.Descriptor))
                );
            m_field.EndUpdateFieldData();

            m_screenManager.ScreenFront(toScreen, m_loadingScreen);
        }

        void DelayedExit(BaseScreen fromScreen)
        {
            if (Params.Exit == null)
                return;

            fromScreen.IsProcessInput = false;
            m_timer.Add(GameAnimations.DefaultDurationX2, Params.Exit);
        }

        async Task SaveFieldDataAndExit()
        {
            if (Params.Exit == null)
                return;

            m_field.BeginUpdateFieldData();
            await Task.WhenAll(
                Task.Delay(150),
                Task.Run(() => m_profile.SaveFieldData(m_field.GetFieldData(), m_field.Descriptor))
                );

            Params.Exit();
        }

    }

}
