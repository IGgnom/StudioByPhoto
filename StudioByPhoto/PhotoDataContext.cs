using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioByPhoto
{
    class PhotoDataContext
    {
        public static StudioByPhotoEntities DBContext { get; } = new StudioByPhotoEntities();
    }
}
