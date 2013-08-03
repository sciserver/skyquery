using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
	public enum JoinMethod
	{
        NA,
        Inner,
        LeftOuter,
        RightOuter,
        Cross
	}
}
