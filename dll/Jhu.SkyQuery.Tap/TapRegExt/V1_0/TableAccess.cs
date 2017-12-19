using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.TapRegExt.V1_0
{
    public class TableAccess
    {
        public DataModelType[] DataModelList { get; set; }
        public Language[] LanguageList { get; set; }
        public OutputFormat[] OutputFormatList { get; set; }
        public UploadMethod[] UploadMethodList { get; set; }
        public TimeLimits RetentionPeriod { get; set; }
        public TimeLimits ExecutionDuration { get; set; }
        public DataLimits OutputLimit { get; set; }
        public DataLimits UploadLimit { get; set; }
    }
}
