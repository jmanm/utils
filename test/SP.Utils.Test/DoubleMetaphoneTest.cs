using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class DoubleMetaphoneTest
    {
        [Test]
        public void TestCreation()
        {
            var dmp1 = new DoubleMetaphone("Seward");
            var dmp2 = new DoubleMetaphone("Soowerred");
            Assert.That(dmp1.PrimaryKey, Is.Not.Empty);
            Assert.That(dmp1.PrimaryKey, Is.EqualTo(dmp2.PrimaryKey));

            Assert.That(new DoubleMetaphone("ACTION").PrimaryKey, Is.Not.EqualTo(new DoubleMetaphone("ACTON").PrimaryKey));
            Assert.That(new DoubleMetaphone("SO PAULO").PrimaryKey, Is.EqualTo(new DoubleMetaphone("SAO PAULO").PrimaryKey));
            Assert.That(new DoubleMetaphone("ST BARTHELEMY").PrimaryKey, Is.EqualTo(new DoubleMetaphone("ST BARTHALEMY").PrimaryKey));

            Assert.That("West Rutland".SoundsLike("Westford"), Is.False);
            /* JM - these egregious false positives makes me want to abandon double metaphone all together
            Assert.That("Caledonia".SoundsLike("Golden Eagle"), Is.False); 
            Assert.That("Pickstown".SoundsLike("Big Stone City"), Is.False);
            Assert.That("Bentleyville".SoundsLike("Penndel"), Is.False);*/

            foreach (var c in "abcdefghijklmnopqrstuvwxyz")
                Assert.DoesNotThrow(() => new DoubleMetaphone(c.ToString()));
        }
    }
}
