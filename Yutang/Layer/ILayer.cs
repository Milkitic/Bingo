using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yutang.Layer
{
    public interface ILayer
    {
        void Measure();
        void Draw();
        void Dispose();
    }
}
