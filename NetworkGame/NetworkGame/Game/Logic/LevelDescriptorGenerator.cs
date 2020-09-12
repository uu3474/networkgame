using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Logic
{
    public class LevelDescriptorGenerator
    {
        const int startSquare = 25;

        int m_levelIndex;
        int m_square;
        List<int> m_widthQueue;
        int m_lastHeightIndex;
        int m_lastDifficult;

        public LevelDescriptorGenerator()
        {
            this.m_levelIndex = 0;
            this.m_square = -1;
            this.m_widthQueue = new List<int>();
            this.m_lastHeightIndex = 0;
            this.m_lastDifficult = 0;
        }

        public LevelDescriptor GetNextLevel()
        {
            var descriptor = ((m_levelIndex < 3) ? new LevelDescriptor(m_levelIndex, m_levelIndex + 2, m_levelIndex + 2, 0) : GenerateNextLevel());
            m_levelIndex++;
            return descriptor;
        }

        LevelDescriptor GenerateNextLevel()
        {
            if(m_widthQueue.Count == m_lastHeightIndex)
            {
                m_widthQueue.Clear();
                m_lastHeightIndex = 0;
                m_lastDifficult = 0;
                while (m_widthQueue.Count == 0)
                {
                    m_square = ((m_square < 0) ? startSquare : (m_square + 1));
                    int border = (int)Math.Ceiling(Math.Sqrt(m_square)) + 1;
                    for (int i = 3; i <= border; i++)
                    {
                        if ((m_square % i) == 0)
                        {
                            int estWidth = i;
                            int estHeight = (m_square / i);
                            if (estWidth < estHeight)
                                continue;

                            int min = Math.Min(estWidth, estHeight);
                            int max = Math.Max(estWidth, estHeight);
                            if (min * 2 < max)
                                continue;

                            m_widthQueue.Add(i);
                        }
                    }
                }
            }

            int height = m_widthQueue[m_lastHeightIndex];
            var descriptor = new LevelDescriptor(m_levelIndex, m_square / height, height, m_lastDifficult);

            m_lastDifficult++;
            if(m_lastDifficult == LevelDescriptor.DifficultsCount)
            {
                m_lastDifficult = 0;
                m_lastHeightIndex++;
            }

            return descriptor;
        }
    }
}
