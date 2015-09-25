namespace IMPression
{
    public abstract class EquationValue : EquationElement
    {
        protected bool m_signed;
        private Complex _value;

        public virtual Complex Value
        {
            get { return _value; }
            set { _value = value;
                if(_value < 0)
                {
                    _value = MathFunctions.Abs(_value);
                    m_signed = true;
                }
                else
                {
                    m_signed = false;
                }
                m_sb.Clear();
                m_sb.Append(_value);
            }
        }

        public virtual bool Signed
        {
            get { return m_signed; }
            set
            {
                m_signed = value;
                if (m_signed && _value > 0)
                {
                    _value = -_value;
                    return;
                }
                if (!m_signed && Value < 0)
                {
                    _value = MathFunctions.Abs(_value);
                }
            }
        }

        public virtual bool CanBeSigned => true;
    }
}