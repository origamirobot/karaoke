using Karaoke.CDG.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a CDG packet that contains a TileBlock or TileBloxkXOR instruction. 
    /// <para>This instruction fills the CDG canvas cell identified by the Row and Column arguments with the 6 by 12 pixel graphics. 
    /// Each pixel that has the corresponding bit cleared (0) in the Pixels argument are assigned the color identified by the BColor argument, while each pixel that is set (1) is assigned the color FColor.
    /// </para>
    /// <para>The Tile Block XOR instruction executes in exactly the same way as the Tile Block Normal instruction, except for how colors are applied to the cell pixels. While the Tile Block Normal instruction 
    /// writes the new color indexes directly into the cell pixels, the Tile Block XOR instruction result in the existing color index for each cell pixel being xor'ed with the new color index. The resulting color 
    /// index from the xor operation is then written back into the cell pixels.</para>
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    /// <see href="http://www.cdgfix.com/help/3.x/Technical_information/The_CDG_graphics_format.htm#Tile%20Block%20Normal"/>
    public class TileBlockPacket : Packet
    {

        #region CONSTANTS


        /// <summary>
        /// The tile block width
        /// </summary>
        public const Int32 TileWidth = 6;

        /// <summary>
        /// The tile block height
        /// </summary>
        public const Int32 TileHeight = 12;

        /// <summary>
        /// The data offset to use when setting the off color index field.
        /// </summary>
        public const Int32 OffColorIndexOffset = 0;

        /// <summary>
        /// The data offset to use when setting the on color index field.
        /// </summary>
        public const Int32 OnColorIndexOffset = 1;

        /// <summary>
        /// The data offset to use when setting the row field.
        /// </summary>
        public const Int32 RowOffset = 2;

        /// <summary>
        /// The data offset to use when setting the column field.
        /// </summary>
        public const Int32 ColumnOffset = 3;

        /// <summary>
        /// The data offset to use when setting the pixels field.
        /// </summary>
        public const Int32 PixelOffset = 4;


        #endregion CONSTANTS

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => (Type == TileBlockType.Normal) ? Instruction.TileBlock : Instruction.TileBlockXOR;

        /// <summary>
        /// Gets or sets the type of tile block.
        /// </summary>
        public TileBlockType Type { get; set; }

        /// <summary>
        /// Gets or sets the color of the on.
        /// </summary>
        public Int16 OnColorIndex { get; set; }

        /// <summary>
        /// Gets or sets the color of the off.
        /// </summary>
        public Int16 OffColorIndex { get; set; }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        public Int16 Row { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        public Int16 Column { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has null pixels.
        /// </summary>
        public Boolean HasNullPixels { get; set; }

        /// <summary>
        /// Gets or sets the pixels inside this block.
        /// </summary>
        public Boolean[,] Pixels { get; set; }

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public IList<ValidationResult> ValidationResults { get; set; }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="TileBlockPacket" /> class.
        /// </summary>
        /// <param name="type">The type of tile block.</param>
        public TileBlockPacket(TileBlockType type)
        {
            Type = type;
            Pixels = new Boolean[TileHeight, TileWidth];
            ValidationResults = new List<ValidationResult>();
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Sets the pixels using a string pattern. Everything thats not a space in the string will be marked as a pixel.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public void SetPixels(String pattern)
        {
            var lines = pattern.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length > TileHeight)
                throw new ArgumentException($"Pattern height must be {TileHeight} pixels");

            for (var y = 0; y < TileHeight; y++)
            {
                var pixels = lines[y].ToArray();
                if (pixels.Length > TileWidth)
                    throw new ArgumentException($"Pattern width must be {TileWidth} pixels");

                for (var x = 0; x < TileWidth; x++)
                    Pixels[y, x] = pixels[x] != ' ';
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            return Type == TileBlockType.Normal
                ? "Tile Block Normal"
                : "Tile Block XOR";
        }

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();

            result[Packet.PacketDataOffset + TileBlockPacket.OffColorIndexOffset] = (Byte)OffColorIndex;
            result[Packet.PacketDataOffset + TileBlockPacket.OnColorIndexOffset] = (Byte)OnColorIndex;
            result[Packet.PacketDataOffset + TileBlockPacket.RowOffset] = (Byte)Row;
            result[Packet.PacketDataOffset + TileBlockPacket.ColumnOffset] = (Byte)Column;

            for (var row = 0; row < TileHeight; row++)
            {
                var rowByte = new Byte();
                for (var col = 0; col < TileWidth; col++)
                {
                    var bit = 0x20 >> col;
                    var on = Pixels[row, col];
                    rowByte = on
                        ? (Byte)(rowByte | bit)
                        : (Byte)(rowByte & ~bit);
                }
                result[Packet.PacketDataOffset + TileBlockPacket.PixelOffset + row] = rowByte;
            }
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new TileBlockPacket(this.Type) { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }

        /// <summary>
        /// Validates the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(Rules rules)

        {
            var list = new List<ValidationResult>();

            if (Column > Packet.MaxColumns)
                list.Add(new ValidationResult(this, $"TileBlock: Invalid X ({Column})"));

            if (Row > MaxRows)
                list.Add(new ValidationResult(this, $"TileBlock: Invalid Y ({Row})"));

            if (OffColorIndex > MaxColors)
                list.Add(new ValidationResult(this, $"TileBlock: Invalid off color ({OffColorIndex})"));

            if (OnColorIndex > MaxColors)
                list.Add(new ValidationResult(this, $"TileBlock: Invalid on color ({OnColorIndex})"));


            //RepairColors(rules, ColorType.OnColor);
            RepairColors(rules, ColorType.OffColor);
            RepairLocation(rules);

            return list;
        }


        #endregion PUBLIC METHODS

        #region PROTECTED METHODS


        /// <summary>
        /// Repairs the location.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="results">The results.</param>
        protected void RepairLocation(Rules rules)
        {
            var isPrimary = rules.PrimaryColors.Contains(OnColorIndex);

            TileBlockPacket? previousPacket = null;
            TileBlockPacket? nextPacket = null;

            if (Column > MaxColumns)
            {
                previousPacket = FindPreviousPacket(rules);
                nextPacket = FindNextPacket(rules);

                if (previousPacket != null)
                {
                    var fixedColumn = isPrimary
                        ? previousPacket.Column + 1
                        : previousPacket.Column;

                    ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid Column: {Column}. Changing to: {previousPacket.Column}"));
                    Column = Convert.ToInt16(fixedColumn);
                }
                else
                {
                    ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid Column: {Column}. No suitable fix found"));
                }
            }

            if (Row > MaxRows)
            {
                if (previousPacket == null)
                    previousPacket = FindPreviousPacket(rules);

                if (previousPacket != null)
                {
                    var fixedRow = previousPacket.Row;
                    ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid Row: {Row}. Changing to: {previousPacket.Row}"));
                    Row = Convert.ToInt16(fixedRow);
                }
                else
                {
                    ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid Row: {Row}. No suitable fix found"));
                }
            }

        }

        /// <summary>
        /// Repairs the colors.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="type">The type.</param>
        protected void RepairColors(Rules rules, ColorType type)
        {
            var colorToRepair = type == ColorType.OnColor
                ? OnColorIndex
                : OffColorIndex;

            var validColors = new List<Int16>();
            validColors.AddRange(type == ColorType.OnColor ? rules.PrimaryColors : rules.BackgroundColors);
            if (type == ColorType.OnColor)
                validColors.AddRange(rules.HighlightColors);

            if (validColors.Contains(colorToRepair))
                return;

            // THIS COLOR IS NOT VALID
            if (validColors.Count == 1)
            {
                // THERES REALLY ONLY ONE VALID COLOR OPTION
                if (type == ColorType.OnColor)
                    OnColorIndex = validColors[0];
                else
                    OffColorIndex = validColors[0];

                ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid {(type == ColorType.OnColor ? "On" : "Off")} Color: {colorToRepair}. Changing to: {validColors[0]}"));
            }
            else
            {
                // THERE IS MORE THAN ONE OPTION FOR FINDING CORRECT COLOR, CONSULT OTHER PACKETS
                var nextTile = FindNextPacket(rules);
                if (nextTile != null)
                {
                    var repairColor = type == ColorType.OnColor
                        ? nextTile.OnColorIndex
                        : nextTile.OffColorIndex;
                    if (validColors.Contains(repairColor))
                    {
                        if (type == ColorType.OnColor)
                            OnColorIndex = repairColor;
                        else
                            OffColorIndex = repairColor;

                        ValidationResults.Add(new ValidationResult(this, $"TileBlock: Invalid {(type == ColorType.OnColor ? "On" : "Off")} Color: {colorToRepair}. Changing to: {repairColor}"));
                    }
                }
            }

        }


        #endregion PROTECTED METHODS

        #region STATIC METHODS



        /// <summary>
        /// Finds the previous packet.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public TileBlockPacket? FindPreviousPacket(Rules rules)
        {
            if (Index == null)
                return null;

            var previousIndex = rules.CdgFile.FindPrevious(Instruction.TileBlock, Index.Value - 1);
            if (previousIndex > -1 && previousIndex < rules.CdgFile.Packets.Count)
                return rules.CdgFile.Packets[previousIndex] as TileBlockPacket;

            return null;
        }

        /// <summary>
        /// Finds the next packet.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public TileBlockPacket? FindNextPacket(Rules rules)
        {
            if (Index == null)
                return null;

            var nextIndex = rules.CdgFile.FindNext(Instruction.TileBlock, Index.Value + 1);
            if (nextIndex > -1 && nextIndex < rules.CdgFile.Packets.Count)
                return rules.CdgFile.Packets[nextIndex] as TileBlockPacket;

            return null;
        }


        #endregion STATIC METHODS

    }

}
