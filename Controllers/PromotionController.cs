using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

public class PromotionController : Controller
{
    private readonly IEventRepository _eventRepository;
    private readonly IPromotionRepository _promotionRepository;

    public PromotionController(IPromotionRepository promotionRepository, IEventRepository eventRepository)
    {
        _promotionRepository = promotionRepository;
        _eventRepository = eventRepository;
    }

    // GET: Promotion
    public IActionResult Index()
    {
        var promotions = _promotionRepository.GetAll();
        return View(promotions);
    }

    // GET: Promotion/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null) return NotFound();

        var promotion = _promotionRepository.GetById(id);
        if (promotion == null) return NotFound();

        return View(promotion);
    }

    // GET: Promotion/Create
    public IActionResult Create()
    {
        ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title");
        return View();
    }

    // POST: Promotion/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(
        [Bind("PromotionID,EventID,PromoCode,DiscountType,DiscountValue,StartDate,EndDate,IsActive")]
        Promotion promotion)
    {
        if (ModelState.IsValid)
        {
            _promotionRepository.Insert(promotion);
            _promotionRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", promotion.EventID);
        return View(promotion);
    }

    // GET: Promotion/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var promotion = _promotionRepository.GetById(id);
        if (promotion == null) return NotFound();
        ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", promotion.EventID);
        return View(promotion);
    }

    // POST: Promotion/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id,
        [Bind("PromotionID,EventID,PromoCode,DiscountType,DiscountValue,StartDate,EndDate,IsActive")]
        Promotion promotion)
    {
        if (id != promotion.PromotionID) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _promotionRepository.Update(promotion);
                _promotionRepository.Save();
            }
            catch (Exception)
            {
                if (!PromotionExists(promotion.PromotionID)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", promotion.EventID);
        return View(promotion);
    }

    // GET: Promotion/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var promotion = _promotionRepository.GetById(id);
        if (promotion == null) return NotFound();

        return View(promotion);
    }

    // POST: Promotion/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var promotion = _promotionRepository.GetById(id);
        if (promotion != null)
        {
            _promotionRepository.Delete(id);
            _promotionRepository.Save();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PromotionExists(int id)
    {
        return _promotionRepository.GetById(id) != null;
    }
}