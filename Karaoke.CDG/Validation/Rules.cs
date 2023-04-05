using Karaoke.CDG.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.Validation
{

    /// <summary>
    /// A set of rules that are implied while reading a CDG file.
    /// </summary>
    public class Rules
    {

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the primary colors.
        /// </summary>
        public List<Int16> PrimaryColors { get; private set; }

        /// <summary>
        /// Gets the highlight colors.
        /// </summary>
        public List<Int16> HighlightColors { get; private set; }

        /// <summary>
        /// Gets the background colors.
        /// </summary>
        public List<Int16> BackgroundColors { get; private set; }

        /// <summary>
        /// Gets the CDG file.
        /// </summary>
        public CdgFile CdgFile { get; private set; }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS



        /// <summary>
        /// Initializes a new instance of the <see cref="Rules" /> class.
        /// </summary>
        public Rules(CdgFile cdgFile)
        {
            CdgFile = cdgFile;
            PrimaryColors = new List<Int16>();
            HighlightColors = new List<Int16>();
            BackgroundColors = new List<Int16>();

            PrimaryColors.Add(1);
            HighlightColors.Add(10);
            BackgroundColors.Add(0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rules" /> class.
        /// </summary>
        /// <param name="cdgFile">The CDG file.</param>
        /// <param name="primaryColors">The primary colors.</param>
        /// <param name="highlightColors">The highlight colors.</param>
        /// <param name="backgroundColors">The background colors.</param>
        public Rules(
            CdgFile cdgFile,
            List<Int16> primaryColors,
            List<Int16> highlightColors,
            List<Int16> backgroundColors)
            : this(cdgFile)
        {

            PrimaryColors = primaryColors;
            HighlightColors = highlightColors;
            BackgroundColors = backgroundColors;
        }


        #endregion CONSTRUCTORS

    }

}
