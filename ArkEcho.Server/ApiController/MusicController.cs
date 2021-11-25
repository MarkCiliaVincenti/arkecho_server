﻿using ArkEcho.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ArkEcho.Server
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        ArkEchoServer server = ArkEchoServer.Instance;

        // GET: api/Music
        [HttpGet]
        public async Task<ActionResult> GetMusicLibrary()
        {
            string lib = await server.GetMusicLibraryString();

            if (server.ServerConfig.Compression)
                lib = await ZipCompression.ZipToBase64(lib);
            else
                lib = lib.GetByteArray().ToBase64();

            return Ok(lib);
        }

        // GET: api/Music/[GUID]
        [HttpGet("{guid}")]
        public async Task<ActionResult> GetMusicFile(Guid guid)
        {
            // TODO: Logging!
            if (guid == Guid.Empty)
                return BadRequest();

            MusicFile musicFile = server.GetMusicFile(guid);

            if (musicFile == null)
                return BadRequest();

            byte[] content = await System.IO.File.ReadAllBytesAsync(musicFile.GetFullFilePath());
            if (server.ServerConfig.Compression)
                content = await ZipCompression.Zip(content);

            FileContentResult result = new FileContentResult(content, $"application/{musicFile.FileFormat}");
            result.FileDownloadName = Path.GetFileName(musicFile.FileName);

            return result;
        }

        // PUT: api/MusicFiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMusicFile(long id, MusicFile musicFile)
        //{
        //    if (id != musicFile.ID)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(musicFile).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MusicFileExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/MusicFiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<MusicFile>> PostMusicFile(MusicFile musicFile)
        //{
        //    context.MusicFiles.Add(musicFile);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction("GetMusicFile", new { id = musicFile.ID }, musicFile);
        //}

        // DELETE: api/MusicFiles/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<MusicFile>> DeleteMusicFile(long id)
        //{
        //    var musicFile = await context.MusicFiles.FindAsync(id);
        //    if (musicFile == null)
        //    {
        //        return NotFound();
        //    }

        //    context.MusicFiles.Remove(musicFile);
        //    await context.SaveChangesAsync();

        //    return musicFile;
        //}

        //private bool MusicFileExists(long id)
        //{
        //    return context.MusicFiles.Any(e => e.ID == id);
        //}
    }
}
