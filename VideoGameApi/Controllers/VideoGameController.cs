using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGameApi.Data;
using VideoGameApi.Models;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly VideoGameDbContext _context;

        public VideoGameController(VideoGameDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            return Ok(await _context.VideoGames
                .Include(g => g.VideoGameDetails)
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Genres)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var game = await _context.VideoGames
                .Include(g => g.VideoGameDetails)
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game is null)
                return NotFound();

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGame>> AddVideoGame(VideoGame newGame)
        {
            if (newGame is null)
                return BadRequest();

            _context.VideoGames.Add(newGame);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideoGameById), new { id = newGame.Id }, newGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id, VideoGame updatedGame)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
                return NotFound();

            // Update scalar properties and FK ids to avoid accidental inserts
            game.Title = updatedGame.Title;
            game.Platform = updatedGame.Platform;
            game.DeveloperId = updatedGame.DeveloperId;
            game.PublisherId = updatedGame.PublisherId;

            // Optional: update genres by syncing ids if provided
            if (updatedGame.Genres != null)
            {
                var genreIds = updatedGame.Genres.Select(g => g.Id).ToList();
                var genres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
                game.Genres = genres;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
                return NotFound();

            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
