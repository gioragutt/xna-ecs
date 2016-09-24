using System;
using WpfServer.Windows;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace WpfServer.Models
{
    public class PlayerInformation : NotifyPropertyChangedBase
    {
        #region Id

        private Guid id;

        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {

                if (id == value)
                    return;

                id = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Transform

        private Transform transform;

        public Transform Transform
        {
            get
            {
                return transform;
            }
            set
            {

                if (transform == value)
                    return;

                transform = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Attributes

        private PlayerAttributes attributes;

        public PlayerAttributes Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                if (attributes == value)
                    return;

                attributes = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public void Update(GameObject gameObject)
        {
            Transform = gameObject.Transform;
            Attributes = gameObject.Components.Get<PlayerAttributes>();
        }
    }
}
