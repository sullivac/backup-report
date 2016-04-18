using Xunit;

namespace BackupReport
{
    public class BackupManifestItemTest
    {
        public class IsMatchTest
        {
            [Fact]
            public void IsMatch_WhenBackupTargetIsNull_ReturnFalse()
            {
                var sut = new BackupManifestItem("any", "any");

                bool result = sut.IsMatch(null);

                Assert.False(result);
            }

            [Fact]
            public void IsMatch_WhenBackupTargetNameIsSame_ReturnTrue()
            {
                var sut = new BackupManifestItem("any", "same");

                bool result = sut.IsMatch("same");

                Assert.True(result);
            }

            [Fact]
            public void IsMatch_WhenBackupTargetNameIsDifferent_ReturnFalse()
            {
                var sut = new BackupManifestItem("any", "name");

                bool result = sut.IsMatch("different");

                Assert.False(result);
            }
        }
    }
}
