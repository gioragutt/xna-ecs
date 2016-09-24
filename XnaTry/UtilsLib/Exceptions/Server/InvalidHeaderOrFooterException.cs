using UtilsLib.Exceptions.Common;

namespace UtilsLib.Exceptions.Server
{
    public class InvalidHeaderOrFooterException : BaseGameException
    {
        public uint ActualHeader
        {
            get;
        }
        public uint ActualFooter
        {
            get;
        }
        public uint ExpectedHeader
        {
            get;
        }
        public uint ExpectedFooter
        {
            get;
        }

        public InvalidHeaderOrFooterException(
            uint actualHeader,
            uint actualFooter,
            uint expectedHeader,
            uint expectedFooter)
            : base(FormatExceptionMessage(actualHeader, actualFooter, expectedHeader, expectedFooter))
        {
            ActualHeader = actualHeader;
            ActualFooter = actualFooter;
            ExpectedHeader = expectedHeader;
            ExpectedFooter = expectedFooter;
        }

        private static string FormatExceptionMessage(
            uint actualHeader,
            uint actualFooter,
            uint expectedHeader,
            uint expectedFooter)
        {
            return string.Format("Invalid header({0} should be {1}) or footer({2} should be {3})", actualHeader,
                expectedHeader, actualFooter, expectedFooter);
        }
    }
}