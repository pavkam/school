using System;
using System.Collections.Generic;
using System.Text;

namespace DCalc.Algorithms
{
    /// <summary>
    /// Simple class to use while populating UI controls. This class is thread-safe.
    /// </summary>
    /// <typeparam name="T">Type of the tagged object.</typeparam>
    public class Tagger<T>
    {
        #region Private Fields

        private String m_Text;
        private T m_Object; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tagger&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="obj">The object.</param>
        public Tagger(String text, T obj)
        {
            m_Text = text;
            m_Object = obj;
        } 

        #endregion

        #region Tagger Public Properties

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object
        {
            get { return m_Object; }
        } 

        #endregion

        #region Tagger Public Methods

        public override String ToString()
        {
            return m_Text;
        } 

        #endregion
    }
}
