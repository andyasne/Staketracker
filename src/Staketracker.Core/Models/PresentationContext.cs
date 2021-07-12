using System;

namespace Staketracker.Core.Models
{
    public class PresentationContext<T>
    {
        public PresentationContext(T model, PresentationMode mode)
        {
            _model = model;
            _mode = mode;
        }


        public PresentationContext(T model, PresentationMode mode, int primaryKey)
        {
            _model = model;
            _mode = mode;
            _primaryKey = primaryKey;
        }
        private T _model;
        private PresentationMode _mode;
        private int _primaryKey;

        public T Model => _model;
        public PresentationMode Mode => _mode;
        public int PrimaryKey => _primaryKey;


    }

    public enum PresentationMode
    {
        Read,
        Create,
        Edit
    }
}
