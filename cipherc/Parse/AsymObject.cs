using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Cipher;

namespace CipherTool.Parse
{
    public class AsymObject<T> : BaseObject, IObject
        where T : IAsym
    {


        protected override void SelfParse(Parser parser) { }
    }
}
