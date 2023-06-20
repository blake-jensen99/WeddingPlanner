using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;

    private MyContext _context; 

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [SessionCheck]
    [HttpGet("weddings/dashboard")]
    public IActionResult Dash()
    {
        List<Wedding> allWeddings = _context.Weddings.Include(w => w.Creator).Include(w => w.Attendees).ThenInclude(a => a.User).ToList();
        return View("Dash", allWeddings);
    }

    [SessionCheck]
    [HttpGet("weddings/{id}")]
    public IActionResult OneWedding(int id)
    {
        Wedding? oneWedding = _context.Weddings.Include(w => w.Attendees).ThenInclude(a => a.User).FirstOrDefault(w => w.WeddingId == id);
        return View("OneWedding", oneWedding);
    }

    [SessionCheck]
    [HttpGet("weddings/addwedding")]
    public IActionResult AddWedding()
    {
        return View("AddWedding");
    }

    [SessionCheck]
    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            return View("AddWedding");
        }
        else 
        {
            newWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Dash");
        }
    }

    [SessionCheck]
    [HttpPost("weddings/rsvp")]
    public IActionResult RSVP(Association rsvp)
    {
        rsvp.UserId = (int)HttpContext.Session.GetInt32("UserId");
        _context.Add(rsvp);
        _context.SaveChanges();
        return RedirectToAction("Dash");
    }

    [SessionCheck]
    [HttpPost("weddings/unrsvp")]
    public IActionResult UNRSVP(Association unrsvp)
    {
        Association? assToDelete = _context.Associations.SingleOrDefault(w => w.AssociationId== unrsvp.AssociationId);
        _context.Associations.Remove(assToDelete);
        _context.SaveChanges();
        return RedirectToAction("Dash");
    }

    [SessionCheck]
    [HttpPost("weddings/destroy")]
    public IActionResult DestroyWedding(Wedding wed)
    {
        Wedding? wedToDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == wed.WeddingId);
        _context.Weddings.Remove(wedToDelete);
        _context.SaveChanges();
        return RedirectToAction("Dash");
    }

    [SessionCheck]
    [HttpGet("weddings/edit/{id}")]
    public IActionResult EditWedding(int id)
    {
        Wedding? OneWedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == id);
        return View("EditWedding", OneWedding);
    }

    [HttpPost("weddings/update/{id}")]
    public IActionResult UpdateWedding(Wedding newWedding, int id)
    {
        Wedding? OldWedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == id);
        if(ModelState.IsValid)
        {
            OldWedding.WedderOne = newWedding.WedderOne;
            OldWedding.WedderTwo = newWedding.WedderTwo;
            OldWedding.Date = newWedding.Date;
            OldWedding.Address = newWedding.Address;
            OldWedding.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("Dash");
        }
        else {
            return View("EditWedding", OldWedding);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}