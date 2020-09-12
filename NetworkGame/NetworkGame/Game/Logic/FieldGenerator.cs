using NetworkGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkGame.Logic
{
    public static class FieldGenerator
    {
        static void Mix(List<CellData> list)
        {
            if (list.Count > 1)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    var newIndex = StaticRandom.Next(list.Count);
                    if (newIndex == i)
                        continue;

                    var tempCell = list[i];
                    list[i] = list[newIndex];
                    list[newIndex] = tempCell;
                }
            }
        }

        static void SetSideStataToAdjacentCells(CellData cell1, CellData cell2, SideState sideState)
        {
            if (cell1.XIndex > cell2.XIndex)
            {
                cell2.RightSide = sideState;
                cell1.LeftSide = sideState;
            }
            else if (cell1.YIndex > cell2.YIndex)
            {
                cell2.BottomSide = sideState;
                cell1.TopSide = sideState;
            }
            else if (cell1.XIndex < cell2.XIndex)
            {
                cell2.LeftSide = sideState;
                cell1.RightSide = sideState;
            }
            else if (cell1.YIndex < cell2.YIndex)
            {
                cell2.TopSide = sideState;
                cell1.BottomSide = sideState;
            }
        }

        static void ApplyToAdjacentCells(CellData[,] cells, CellData cell, Action<CellData, CellData> adjacentCellAction)
        {
            //rightCell
            if (cell.XIndex < cells.GetLength(0) - 1)
                adjacentCellAction(cell, cells[cell.XIndex + 1, cell.YIndex]);
            //bottomCell
            if (cell.YIndex < cells.GetLength(1) - 1)
                adjacentCellAction(cell, cells[cell.XIndex, cell.YIndex + 1]);
            //leftCell
            if (cell.XIndex > 0)
                adjacentCellAction(cell, cells[cell.XIndex - 1, cell.YIndex]);
            //topCell
            if (cell.YIndex > 0)
                adjacentCellAction(cell, cells[cell.XIndex, cell.YIndex - 1]);
        }

        static int GetSidesCount(CellData cell)
        {
            int sidesCount = 0;
            if (cell.RightSide != SideState.Empty)
                ++sidesCount;
            if (cell.BottomSide != SideState.Empty)
                ++sidesCount;
            if (cell.LeftSide != SideState.Empty)
                ++sidesCount;
            if (cell.TopSide != SideState.Empty)
                ++sidesCount;

            return sidesCount;
        }

        static CellAngle DetectAngle(CellData cell)
        {
            switch (cell.Wire)
            {
                case WireType.End:
                    {
                        if (cell.RightSide != SideState.Empty)
                            return CellAngle.Angle0;
                        else if (cell.BottomSide != SideState.Empty)
                            return CellAngle.Angle90;
                        else if (cell.LeftSide != SideState.Empty)
                            return CellAngle.Angle180;
                        else if (cell.TopSide != SideState.Empty)
                            return CellAngle.Angle270;
                    }
                    break;
                case WireType.Line:
                    {
                        if (cell.RightSide != SideState.Empty && cell.LeftSide != SideState.Empty)
                            return CellAngle.Angle0;
                        else if (cell.BottomSide != SideState.Empty && cell.TopSide != SideState.Empty)
                            return CellAngle.Angle90;
                    }
                    break;
                case WireType.Angle2:
                    {
                        if (cell.RightSide != SideState.Empty && cell.BottomSide != SideState.Empty)
                            return CellAngle.Angle0;
                        else if (cell.BottomSide != SideState.Empty && cell.LeftSide != SideState.Empty)
                            return CellAngle.Angle90;
                        else if (cell.LeftSide != SideState.Empty && cell.TopSide != SideState.Empty)
                            return CellAngle.Angle180;
                        else if (cell.TopSide != SideState.Empty && cell.RightSide != SideState.Empty)
                            return CellAngle.Angle270;
                    }
                    break;
                case WireType.Angle3:
                    {
                        if (cell.TopSide == SideState.Empty)
                            return CellAngle.Angle0;
                        else if (cell.RightSide == SideState.Empty)
                            return CellAngle.Angle90;
                        else if (cell.BottomSide == SideState.Empty)
                            return CellAngle.Angle180;
                        else if (cell.LeftSide == SideState.Empty)
                            return CellAngle.Angle270;
                    }
                    break;
            }
            return CellAngle.Angle0;
        }

        static void FinalizeCell(CellData cell, CellData sourceCell)
        {
            int sidesCount = GetSidesCount(cell);
            switch (sidesCount)
            {
                case 1:
                    cell.Wire = WireType.End;
                    break;
                case 2:
                    {
                        cell.Wire = ((cell.RightSide == SideState.Empty && cell.LeftSide == SideState.Empty)
                            || (cell.TopSide == SideState.Empty && cell.BottomSide == SideState.Empty))
                            ? WireType.Line : WireType.Angle2;
                    }
                    break;
                case 3:
                    cell.Wire = WireType.Angle3;
                    break;
                case 4:
                    cell.Wire = WireType.Angle4;
                    break;
            }

            if(cell == sourceCell)
                cell.Content = ContentType.Router;
            else if(cell.Wire == WireType.End)
                cell.Content = ContentType.Display;
            else
                cell.Content = ContentType.Wire;

            cell.InitialAngle = DetectAngle(cell);
            cell.Angle = cell.InitialAngle;
            switch (StaticRandom.Next(3))
            {
                case 0:
                    cell.Rotate90();
                    break;
                case 1:
                    cell.Rotate180();
                    break;
                case 2:
                    cell.Rotate270();
                    break;
            }
        }

        static void GenerateWires(CellData[,] cells, CellData sourceCell, int difficult)
        {
            var generateQueue = new LinkedList<CellData>();
            generateQueue.AddLast(sourceCell);

            var sidesCellsList = new List<CellData>();

            Action<CellData, CellData> clearAdjacentCellConnection = (CellData cell, CellData adjacentCell)
                => SetSideStataToAdjacentCells(cell, adjacentCell, SideState.Empty);

            Action<CellData, CellData> addAdjacentEmptyCell = (CellData cell, CellData adjacentCell) =>
            {
                if (adjacentCell.Content == ContentType.Empty)
                {
                    sidesCellsList.Add(adjacentCell);
                }
                else if (adjacentCell.Content == ContentType.InGeneratorQueue
                    && StaticRandom.Next(LevelDescriptor.DifficultsCount - difficult) == 0)
                {
                    ApplyToAdjacentCells(cells, adjacentCell, clearAdjacentCellConnection);
                    sidesCellsList.Add(adjacentCell);
                }

            };

            while (generateQueue.Count > 0)
            {
                CellData cell = null;
                if (difficult < LevelDescriptor.DifficultsCount / 2)
                {
                    cell = generateQueue.First.Value;
                    generateQueue.RemoveFirst();
                }
                else
                {
                    cell = generateQueue.Last.Value;
                    generateQueue.RemoveLast();
                }

                sidesCellsList.Clear();
                ApplyToAdjacentCells(cells, cell, addAdjacentEmptyCell);
                Mix(sidesCellsList);

                for (int i = 0; i < sidesCellsList.Count; ++i)
                {
                    var connectToCell = sidesCellsList[i];

                    SetSideStataToAdjacentCells(cell, connectToCell, SideState.Disconnected);
                    connectToCell.Content = ContentType.InGeneratorQueue;
                    generateQueue.AddLast(connectToCell);
                }

                cell.Content = ContentType.Generated;
            }
        }

        public static FieldData GenerateFieldData(LevelDescriptor descriptor)
        {
            CellData[,] cells = new CellData[descriptor.Width, descriptor.Height];

            for (int x = 0; x < cells.GetLength(0); x++)
                for (int y = 0; y < cells.GetLength(1); y++)
                    cells[x, y] = new CellData(x, y);


            CellData sourceCell = cells[StaticRandom.Next(descriptor.Width), StaticRandom.Next(descriptor.Height)];
            GenerateWires(cells, sourceCell, descriptor.Difficult);

            for (int x = 0; x < cells.GetLength(0); x++)
                for (int y = 0; y < cells.GetLength(1); y++)
                    FinalizeCell(cells[x, y], sourceCell);

            return new FieldData() { CellsData = cells };
        }
    }
}