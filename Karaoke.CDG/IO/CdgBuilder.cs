using Karaoke.Core.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.IO
{

    /// <inheritdoc />
    /// <summary>
    /// Responsible for turning file locations into <see cref="T:WordSmith.CDG.IO.CdgFile" /> objects.
    /// </summary>
    public interface ICdgBuilder : IDisposable
    {

        /// <summary>
        /// Loads the specified CD+G file and returns the result.
        /// </summary>
        /// <param name="fileName">Name of the CD+G file to load.</param>
        /// <returns></returns>
        CdgFile Load(String fileName);

        /// <summary>
        /// Loads the specified CD+G file and returns the result asynchronously.
        /// </summary>
        /// <param name="fileName">Name of the CD+G file to load.</param>
        /// <returns></returns>
        Task<CdgFile> LoadAsync(String fileName);

        /// <summary>
        /// Saves the specified CD+G file.
        /// </summary>
        /// <param name="file">The CD+G file to save.</param>
        /// <param name="fileName">Use this parameter to save a copy of the file in another location.</param>
        void Save(CdgFile file, String? fileName = null);

        /// <summary>
        /// Saves the specified CD+G file asynchronously.
        /// </summary>
        /// <param name="file">The CD+G file to save.</param>
        /// <param name="fileName">Use this parameter to save a copy of the file in another location.</param>
        /// <returns></returns>
        Task SaveAsync(CdgFile file, String? fileName = null);

    }

    /// <inheritdoc />
    /// <summary>
    /// Responsible for turning file locations into <see cref="T:WordSmith.CDG.IO.CdgFile" /> objects.
    /// </summary>
    public class CdgBuilder : ICdgBuilder
    {

        #region PROTECTED PROPERTIES


        /// <summary>
        /// Gets the file stream.
        /// </summary>
        protected ICdgFileStream FileStream { get; private set; }

        /// <summary>
        /// Gets the file utility.
        /// </summary>
        protected IFileUtility FileUtility { get; private set; }

        /// <summary>
        /// Gets the directory utiluty.
        /// </summary>
        protected IDirectoryUtility DirectoryUtility { get; private set; }

        /// <summary>
        /// Gets the path utility.
        /// </summary>
        protected IPathUtility PathUtility { get; private set; }

        /// <summary>
        /// Gets the packet builder.
        /// </summary>
        protected IPacketBuilder PacketBuilder { get; private set; }


        #endregion PROTECTED PROPERTIES

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="CdgBuilder" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="packetBuilder">The packet builder.</param>
        /// <param name="fileUtility">The file utility.</param>
        /// <param name="directoryUtility">The directory utility.</param>
        /// <param name="pathUtility">The path utility.</param>
        public CdgBuilder(
            ICdgFileStream fileStream,
            IPacketBuilder packetBuilder,
            IFileUtility fileUtility,
            IDirectoryUtility directoryUtility,
            IPathUtility pathUtility)
        {
            FileStream = fileStream;
            PacketBuilder = packetBuilder;
            FileUtility = fileUtility;
            DirectoryUtility = directoryUtility;
            PathUtility = pathUtility;
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <inheritdoc />
        /// <summary>
        /// Loads the specified CD+G file and returns the result.
        /// </summary>
        /// <param name="fileName">Name of the CD+G file to load.</param>
        /// <returns></returns>
        public virtual CdgFile Load(String fileName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new CdgFile(fileName);
            try
            {
                // MAKE SURE THE FILE ACTUALLY EXISTS
                if (!FileUtility.FileExists(fileName))
                    throw new FileNotFoundException($"Could not find the file {fileName}", fileName);

                var info = FileUtility.GetFileInfo(fileName);

                // MAKE SURE THE FILE IS A CDG FILE
                if (!info.Extension.Equals(".cdg", StringComparison.OrdinalIgnoreCase))
                    throw new FileLoadException($"File type must be a CD+G with a .cdg extension", fileName);

                FileStream.Open(fileName);

                var index = 0;
                var packet = PacketBuilder.ReadPacket(FileStream);

                while (packet != null)
                {
                    packet.Index = index;
                    result.Packets.Add(packet);
                    packet = PacketBuilder.ReadPacket(FileStream);
                    index++;
                }
                PacketBuilder.RepairCdg(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stopwatch.Stop();
                FileStream.Seek(0, SeekOrigin.Begin);
                FileStream.Close();
            }
        }

        /// <summary>
        /// Loads the specified CD+G file and returns the result asynchronously.
        /// </summary>
        /// <param name="fileName">Name of the CD+G file to load.</param>
        /// <returns></returns>
        public virtual async Task<CdgFile> LoadAsync(String fileName) => await Task.Run(() => Load(fileName));

        /// <summary>
        /// Saves the specified CD+G file.
        /// </summary>
        /// <param name="file">The CD+G file to save.</param>
        /// <param name="fileName">Use this parameter to save a copy of the file in another location.</param>
        public virtual void Save(CdgFile file, String? fileName = null)
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = file.FileName;

            FileStream.Close();
            var data = new List<Byte>();
            foreach (var packet in file.Packets)
                data.AddRange(packet.Data);

            FileUtility.WriteAllBytes(fileName, data.ToArray());
        }

        /// <summary>
        /// Saves the specified CD+G file asynchronously.
        /// </summary>
        /// <param name="file">The CD+G file to save.</param>
        /// <param name="fileName">Use this parameter to save a copy of the file in another location.</param>
        /// <returns></returns>
        public virtual async Task SaveAsync(CdgFile file, String? fileName = null) => await Task.Run(() => Save(file, fileName));

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => FileStream?.Dispose();


        #endregion PUBLIC METHODS

    }

}
