using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standard.Licensing;
using Standard.Licensing.Validation;

namespace Slithin.Core
{
    public static class Beta
    {
        public static void Activate()
        {
        }

        public static bool HasLicence()
        {
            var licFile = Path.Combine(ServiceLocator.ConfigBaseDir, "licence.lic");

            return File.Exists(licFile);
        }

        public static bool Validate()
        {
            var licFile = Path.Combine(ServiceLocator.ConfigBaseDir, "licence.lic");

            if (File.Exists(licFile))
            {
                var license = License.Load(licFile);
                var validationFailures = license.Validate()
                                .ExpirationDate()
                                .When(lic => lic.Type == LicenseType.Standard)
                                // .And()
                                //.Signature(publicKey)
                                .AssertValidLicense();

                return !validationFailures.Any();
            }

            return false;
        }
    }
}
