namespace GameLive
{
    public class Compare
    {
        public bool IsNotEqualMas(int[,] mas1, int[,] mas2)
        {
            bool isEqual = false;
            if (mas1.GetLength(0) == mas2.GetLength(0) && mas1.GetLength(1) == mas2.GetLength(1))
            {
                for (int row = 0; row < mas1.GetLength(0); row++)
                {
                    for (int column = 0; column < mas1.GetLength(1); column++)
                    {
                        if (mas1[row, column] != mas2[row, column])
                        {
                            isEqual = true;
                            break;
                        }
                        if (isEqual) break;
                    }
                    if (isEqual) break;
                }
            }
            else
            {
               return true;
            }
            return isEqual;
        }
    }
}
