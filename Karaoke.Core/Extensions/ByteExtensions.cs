namespace Karaoke
{

    /// <summary>
    /// Extension methods for <see cref="Byte"/>.
    /// </summary>
    public static class ByteExtensions
    {


        /// <summary>
        /// Converts this byte array to a hexidecimal string representation.
        /// </summary>
        /// <param name="bytes">The bytes this method is extending.</param>
        /// <returns></returns>
        public static String ToHex(this Byte[] bytes)
        {
            Char[] c = new Char[bytes.Length * 2];

            Byte b;

            for (Int32 bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((Byte)(bytes[bx] >> 4));
                c[cx] = (Char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((Byte)(bytes[bx] & 0x0F));
                c[++cx] = (Char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new String(c);
        }

        /// <summary>
        /// Converts a hexidecimal string to a byte array.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static Byte[] HexToBytes(this String str)
        {
            if (str.Length == 0 || str.Length % 2 != 0)
                return new Byte[0];

            Byte[] buffer = new Byte[str.Length / 2];
            Char c;
            for (Int32 bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
            {
                // Convert first half of byte
                c = str[sx];
                buffer[bx] = (Byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

                // Convert second half of byte
                c = str[++sx];
                buffer[bx] |= (Byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
            }

            return buffer;
        }

    }

}
