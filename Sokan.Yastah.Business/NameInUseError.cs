﻿using Sokan.Yastah.Common.OperationModel;

namespace Sokan.Yastah.Business
{
    public class NameInUseError
        : OperationError
    {
        public NameInUseError(string name)
            : base($"Name {name} is already in use") { }
    }
}
