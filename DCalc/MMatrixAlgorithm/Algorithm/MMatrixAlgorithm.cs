using System;
using System.Collections.Generic;
using System.Text;
using DCalcCore.Algorithm;
using DCalcCore.Utilities;

namespace MMatrixAlgorithm.Algorithm
{
    /// <summary>
    /// Matrix multiplication algorithm. This class is thread-safe.
    /// </summary>
    public sealed class MMatrixAlgorithm : IAlgorithm
    {
        #region Private Fields

        private String m_MethodName = "MatrixMultiply";
        private String m_MethodBody =
          @"Object[] MatrixMultiply(params Object[] input)
            {
                Int32 sizeOfRow = (Int32)input[0];
                Int32 countOfRows = (Int32)input[1];
                Object[] results = new Object[countOfRows];

                for (Int32 rowNumber = 0; rowNumber < countOfRows; rowNumber++)
                {
                    Double resultCell = 0;

                    for (Int32 i = 0; i < sizeOfRow; i++)
                    {
                        Double x = ((Double)input[i + 2]);
                        Double y = ((Double)input[((rowNumber + 1) * sizeOfRow) + 2 + i]);

                        resultCell += (x * y);
                    }

                    results[rowNumber] = resultCell;
                }

                return results;
            }";

        /* Settings */
        private Int32 m_Matrix1X, m_Matrix1Y, m_Matrix2X, m_Matrix2Y;
        private Double[,] m_Matrix1, m_Matrix2, m_ResultMatrix;

        #endregion

        #region Constructors

        public MMatrixAlgorithm(Int32 matrix1X, Int32 matrix1Y,
            Int32 matrix2X, Int32 matrix2Y)
        {
            if (matrix1X < 1)
                throw new ArgumentException("matrix1X");

            if (matrix1Y < 1)
                throw new ArgumentException("matrix1Y");

            if (matrix2X < 1)
                throw new ArgumentException("matrix2X");

            if (matrix2Y < 1)
                throw new ArgumentException("matrix2Y");

            if (matrix1X != matrix2Y)
                throw new ArgumentException("matrix1X, matrix2Y");

            m_Matrix1X = matrix1X;
            m_Matrix1Y = matrix1Y;
            m_Matrix2X = matrix2X;
            m_Matrix2Y = matrix2Y;
        } 

        #endregion

        #region MMatrixAlgorithm Public Properties

        /// <summary>
        /// Gets the matrix A.
        /// </summary>
        /// <value>The matrix A.</value>
        public Double[,] MatrixA
        {
            get { return m_Matrix1; }
        }

        /// <summary>
        /// Gets the matrix B.
        /// </summary>
        /// <value>The matrix B.</value>
        public Double[,] MatrixB
        {
            get { return m_Matrix2; }
        }

        /// <summary>
        /// Gets the resulting matrix.
        /// </summary>
        /// <value>The resulting matrix.</value>
        public Double[,] ResultingMatrix
        {
            get { return m_ResultMatrix; }
        } 

        #endregion

        #region IAlgorithm Members

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public String MethodName
        {
            get { return m_MethodName; }
        }

        /// <summary>
        /// Gets the method body.
        /// </summary>
        /// <value>The method body.</value>
        public String MethodBody
        {
            get { return m_MethodBody; }
        }

        /// <summary>
        /// Gets the input set count.
        /// </summary>
        /// <value>The input set count.</value>
        public Int32 InputSetCount
        {
            get 
            {
                /* Each matrix line * column multiplication */
                return (m_Matrix1Y * m_Matrix2X); 
            }
        }

        /// <summary>
        /// Gets the input set.
        /// </summary>
        /// <param name="setNumber">The set number.</param>
        /// <returns></returns>
        public ScalarSet GetInputSet(Int32 setNumber)
        {
            if (setNumber >= InputSetCount || setNumber < 0)
                throw new ArgumentException("setNumber");

            if (m_Matrix1 == null || m_Matrix2 == null)
            {
                /* Matrixes were not initialized! The call to prepare was not done! */
                throw new ArgumentException("Matrixes were not initialized!");
            }

            /* Find the row and column that will be multiplied. */
            Int32 rowFromMatrix1 = (setNumber / m_Matrix2X);
            Int32 colFromMatrix2 = (setNumber % m_Matrix2X);

            /* Set pack (1 Int32 for size of set) + 1 row + 1 column (which are equal) */
            Object[] pack = new Object[2 + (m_Matrix1X * 2)];

            /* Add line size */
            pack[0] = m_Matrix1X;
            pack[1] = 1;

            /* Prepare the scalar set */
            for (Int32 i = 0; i < m_Matrix1X; i++)
            {
                pack[i + 2] = m_Matrix1[i, rowFromMatrix1];
                pack[i + 2 + m_Matrix1X] = m_Matrix2[colFromMatrix2, i];
            }

            return new ScalarSet(setNumber, pack);
        }

        /// <summary>
        /// Returns the output set to the algorithm.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setNumber">The set number.</param>
        public void ReceiveOutputSet(ScalarSet set, Int32 setNumber)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            if (set.Count != 1)
                throw new ArgumentException("set");

            /* Find the row and column this cell */
            Int32 row = (setNumber / m_Matrix2X);
            Int32 col = (setNumber % m_Matrix2X);

            m_ResultMatrix[col, row] = (Double)set[0];
        }

        /// <summary>
        /// Instructs the algorithm to prepare it's data.
        /// </summary>
        public void PrepareToStart()
        {
            Random random = new Random();

            /* Build up those matrixes */
            m_Matrix1 = new Double[m_Matrix1X, m_Matrix1Y];
            m_Matrix2 = new Double[m_Matrix2X, m_Matrix2Y];
            m_ResultMatrix = new Double[m_Matrix2X, m_Matrix1Y];

            /* Populate matrix 1 */
            for (Int32 x = 0; x < m_Matrix1X; x++)
            {
                for (Int32 y = 0; y < m_Matrix1Y; y++ )
                    m_Matrix1[x, y] = Math.Round(random.NextDouble() * 100, 2);
            }

            /* Populate Matrix 2*/
            for (Int32 x = 0; x < m_Matrix2X; x++)
            {
                for (Int32 y = 0; y < m_Matrix2Y; y++)
                    m_Matrix2[x, y] = Math.Round(random.NextDouble() * 100, 2);
            }
        }

        /// <summary>
        /// Instructs the algorithm to processs the result data.
        /// </summary>
        public void PrepareToFinish()
        {
        }

        #endregion
    }
}
