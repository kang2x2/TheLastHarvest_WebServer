using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Data;

namespace WebApi.Controllers
{
    public enum RankingType
    {
        Kill,
        Clear,
        End
    }

    // REST (Repreesentational STate Transfer)
    // 공식 표준 스펙은 아님 
    // 원래 있던 HTTP 통신에서 기능을 재사용해서 자체적인 데이터 송수신 규칙을 만든 것.

    // ApiController 특징
    // 그냥 C# 객체 반환 가능. 만약 null을 반환하면 클라에 204 Response(No Content) 발생.
    // string -> text/plain 반환.
    // 그 외 -> application/josn 반환.

    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        ApplicationDbContext _context;

        public RankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        [HttpPost("adduserdata")]
        public IActionResult AddUserData([FromBody] GameResult gameResult)
        {
            if(Encoding.UTF8.GetByteCount(gameResult.UserId) < 6 || Encoding.UTF8.GetByteCount(gameResult.UserId) > 12)
            {
                return BadRequest("6~12 사이의 길이로 ID를 설정해 주세요.");
            }

            if (Encoding.UTF8.GetByteCount(gameResult.UserName) < 6 || Encoding.UTF8.GetByteCount(gameResult.UserName) > 12)
            {
                return BadRequest("6~12 사이의 길이로 닉네임을 설정해 주세요.");
            }

            if (Encoding.UTF8.GetByteCount(gameResult.UserPassword) < 4 || Encoding.UTF8.GetByteCount(gameResult.UserPassword) > 12)
            {
                return BadRequest("4~12 사이의 길이로 패스워드를 설정해 주세요.");
            }

            // gr은 DB 테이블을 순회하며 담기는 객체를 뜻함.
            // 즉, 인자로 받은 gameResult와 DB의 모든 GameResult 객체를 순회하며 검사.
            var result = _context.GameResults.FirstOrDefault
                (gr => gr.UserId == gameResult.UserId ||
                       gr.UserName == gameResult.UserName);

            if(result != null)
            {
                if(result.UserId == gameResult.UserId)
                {
                    return BadRequest("같은 아이디가 이미 존재합니다.");
                }

                if (result.UserName == gameResult.UserName)
                {
                    return BadRequest("같은 닉네임이 이미 존재합니다.");
                }
            }

            _context.GameResults.Add(gameResult);
            _context.SaveChanges();
            
            return Ok(gameResult);
        }

        // Read
        [HttpGet] // 모든 아이템을 달라.
        public List<GameResult> GetGameResults([FromQuery] RankingType rankingType)
        {
            List<GameResult> results = new List<GameResult>();

            switch (rankingType)
            {
                case RankingType.Kill:
                    results = _context.GameResults.
                        OrderByDescending(item => item.KillScore).ToList();
                    break;
                case RankingType.Clear:
                    results = _context.GameResults.
                        OrderByDescending(item => item.ClearScore).ToList();
                    break;
            }

            return results;
        }

        [HttpGet("{id}")]
        public GameResult GetGameResult(int id)
        {
            GameResult result = _context.GameResults.
                Where(item => item.Id == id).FirstOrDefault();

            return result;
        }

        [HttpGet("getuserdata")]
        public IActionResult GetUserData(string id, string pw)
        {
            var result = _context.GameResults.FirstOrDefault
                (gr => gr.UserId == id);

            if (result == null)
            {
                return BadRequest("존재하지 않는 ID입니다.");
            }

            if (result.UserPassword != pw)
            {
                return BadRequest("비밀번호를 확인해 주세요.");
            }

            return Ok(result);
        }

        // Update
        [HttpPut] // 보안 상의 문제로 웹에서는 활용하지 않는다.
        public bool UpdateGameResult([FromBody]GameResult gameResult)
        {
            // FromBody 클라이언트에서 보낸 데이터를 서버에서 자동으로 변환해 할당한다.
            var findResult = _context.GameResults
                .Where(item => item.Id == gameResult.Id).FirstOrDefault();

            if (findResult == null)
            {
                return false;
            }

            findResult.KillScore = gameResult.KillScore;
            findResult.ClearScore = gameResult.ClearScore;
            findResult.PlayTime = gameResult.PlayTime;
            _context.SaveChanges();

            return true;
        }

        // Delete
        [HttpDelete("{id}")] // 보안 상의 문제로 웹에서는 활용하지 않는다.
        public bool DeleteGameResult(int id)
        {
            var findResult = _context.GameResults.
                Where(item => item.Id == id).FirstOrDefault();

            if(findResult == null)
            {
                return false;
            }

            _context.GameResults.Remove(findResult);
            _context.SaveChanges();

            return true;
        }
    }
}
