using Jays_BackOffice.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jays_BackOffice.Models;
using Jays_BackOffice.DB_Models;

namespace Jays_BackOffice.Controllers
{
    public class ApplyMarkupsController : Controller
    {
        private readonly DB_Context _context;
        private readonly HttpClient _httpClient;


        public ApplyMarkupsController(DB_Context context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var markup = await _context.Markups.Where(e => e.IsActive == true).ToListAsync();
            List<MarkupViewModel> model = new List<MarkupViewModel>();

            foreach (var item in markup)
            {
                #region Working For Fare Type

                var query = from f in _context.FareTypes.Where(e => e.IsActive == true)
                            join mf in _context.MarkupFareTypes
                            on f.FareTypeId equals mf.FareType.FareTypeId into faretypeGroup
                            from mfResult in faretypeGroup.DefaultIfEmpty() // Left Join
                            where mfResult == null || mfResult.Markup.MarkupId == item.MarkupId // Include all faretypes if not mapped
                            select new
                            {
                                FareTypeId = f.FareTypeId,
                                FareTypeName = f.Fare_Type,
                                MarkupId = mfResult != null ? mfResult.Markup.MarkupId : (int?)null,
                                IsSelected = mfResult.MarkupFareId != null ? true : false
                            };
                var FareTypeQuery = query.ToList();
                var faretypeVM = new List<MarkupFareTypeViewModel>();
                foreach (var tiem in FareTypeQuery)
                {
                    faretypeVM.Add(new MarkupFareTypeViewModel
                    {
                        MarkupId = tiem.MarkupId != null ? tiem.MarkupId.Value : 0,
                        FareTypeId = tiem.FareTypeId != null ? tiem.FareTypeId : 0,
                        FareTypeName = tiem.FareTypeName != null ? tiem.FareTypeName : "",
                        IsSelected = tiem.IsSelected != null ? tiem.IsSelected : false
                    });
                }

                #endregion

                #region GDS
                List<int> gdsIds = _context.MarkupGds
                    .Where(mf => mf.Markup.MarkupId == item.MarkupId) // Filter by specific MarkupId
                    .Select(mf => mf.gds.GdsId)
                    .ToList();
                var gdstype = await _context.MarkupGds.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.gds).Select(

                       mg => new MarkupGdsViewModel
                       {
                           MarkupId = mg.Markup.MarkupId,
                           GdsId = mg.gds.GdsId,
                           GdsName = mg.gds.GdsName,
                           IsSelected = true
                       }).ToListAsync();

                var unassociatedGds = _context.GDS.Where(gds => !gdsIds.Contains(gds.GdsId)).ToList();
                foreach (var un in unassociatedGds)
                {
                    gdstype.Add(new MarkupGdsViewModel { GdsId = un.GdsId, GdsName = un.GdsName, IsSelected = false, MarkupId = 0 });
                }
                #endregion

                #region Source
                List<int> SoruceIds = _context.MarkupMarketingSources
                    .Where(mf => mf.Markup.MarkupId == item.MarkupId) // Filter by specific MarkupId
                    .Select(mf => mf.Source.SourceId)
                    .ToList();
                var gdsSource = await _context.MarkupMarketingSources.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Source).Select(

                       mg => new MarkupAdvertiserViewModel
                       {
                           MarkupId = mg.Markup.MarkupId,
                           SourceId = mg.Source.SourceId,
                           SourceName = mg.Source.SourceName,
                           IsSelected = true
                       }).ToListAsync();

                var unassociatedSource = _context.MarketingSources.Where(gds => !SoruceIds.Contains(gds.SourceId)).ToList();
                foreach (var un in unassociatedSource)
                {
                    gdsSource.Add(new MarkupAdvertiserViewModel { SourceId = un.SourceId, MarkupId = 0, SourceName = un.SourceName, IsSelected = false });
                }
                #endregion

                #region Days
                List<int> DayIds = _context.MarkupDay
                        .Where(mf => mf.Markup.MarkupId == item.MarkupId) // Filter by specific MarkupId
                        .Select(mf => mf.Day.DayId)
                        .ToList();
                var gdsDay = await _context.MarkupDay.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Day).Select(

                       mg => new MarkupDayNameViewModel
                       {
                           MarkupId = mg.Markup.MarkupId,
                           DayNameId = mg.Day.DayId,
                           DayName = mg.Day.Day_Name,
                           IsSelected = true
                       }).ToListAsync();

                var unassociatedDay = _context.DayName.Where(gds => !DayIds.Contains(gds.DayId)).ToList();
                foreach (var un in unassociatedDay)
                {
                    gdsDay.Add(new MarkupDayNameViewModel { DayNameId = un.DayId, MarkupId = 0, DayName = un.Day_Name, IsSelected = false });
                }
                #endregion

                #region Journy Type
                List<int> Journytypeids = _context.MarkupJournyTypes
                        .Where(mf => mf.Markup.MarkupId == item.MarkupId) // Filter by specific MarkupId
                        .Select(mf => mf.Journy.JournyTypeId)
                        .ToList();
                var jtyp = await _context.MarkupJournyTypes.ToListAsync();
                var gdsj = await _context.MarkupJournyTypes.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Journy).Select(

                   mg => new MarkupJourneyTypeViewModel
                   {
                       MarkupId = mg.Markup.MarkupId,
                       JourneyTypeId = mg.Journy.JournyTypeId,
                       JourneyType = mg.Journy.JournyType,
                       IsSelected = true
                   }).ToListAsync();

                var unassociatedJ = _context.JourneyTypes.Where(gds => !Journytypeids.Contains(gds.JournyTypeId)).ToList();
                foreach (var un in unassociatedJ)
                {
                    gdsj.Add(new MarkupJourneyTypeViewModel { JourneyTypeId = un.JournyTypeId, MarkupId = 0, JourneyType = un.JournyType, IsSelected = false });
                }
                #endregion

                MarkupViewModel vm = new MarkupViewModel
                {
                    Markup = item,
                    SelectedGDS = gdstype,
                    SelectedFareType = faretypeVM,
                    SelectedSource = gdsSource,
                    SelectedDayName = gdsDay,
                    SelectedJournyType = gdsj

                };
                model.Add(vm);
            }

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                List<MarkupViewModel> model = new List<MarkupViewModel>();

                #region Working For Fare Type

                var faretype = new List<MarkupFareTypeViewModel>();

                var unassociatedfaretype = await _context.FareTypes.Where(gds => gds.IsActive == true).ToListAsync();
                foreach (var un in unassociatedfaretype)
                {
                    faretype.Add(new MarkupFareTypeViewModel { FareTypeId = un.FareTypeId, FareTypeName = un.Fare_Type, IsSelected = false, MarkupId = 0 });
                }
                #endregion

                #region GDS
                var gdstype = new List<MarkupGdsViewModel>();
                var unassociatedGds = _context.GDS.Where(gds => gds.IsActive == true).ToList();
                foreach (var un in unassociatedGds)
                {
                    gdstype.Add(new MarkupGdsViewModel { GdsId = un.GdsId, GdsName = un.GdsName, IsSelected = false, MarkupId = 0 });
                }
                #endregion

                #region Source
                var gdsSource = new List<MarkupAdvertiserViewModel>();

                var unassociatedSource = _context.MarketingSources.Where(gds => gds.IsActive == true).ToList();
                foreach (var un in unassociatedSource)
                {
                    gdsSource.Add(new MarkupAdvertiserViewModel { SourceId = un.SourceId, MarkupId = 0, SourceName = un.SourceName, IsSelected = false });
                }
                #endregion

                #region Days
                var gdsDay = new List<MarkupDayNameViewModel>();

                var unassociatedDay = _context.DayName.Where(gds => gds.IsActive == true).ToList();
                foreach (var un in unassociatedDay)
                {
                    gdsDay.Add(new MarkupDayNameViewModel { DayNameId = un.DayId, MarkupId = 0, DayName = un.Day_Name, IsSelected = false });
                }
                #endregion

                #region Journy Type
                var gdsj = new List<MarkupJourneyTypeViewModel>();

                var unassociatedJ = _context.JourneyTypes.Where(gds => gds.IsActive == true).ToList();
                foreach (var un in unassociatedJ)
                {
                    gdsj.Add(new MarkupJourneyTypeViewModel { JourneyTypeId = un.JournyTypeId, MarkupId = 0, JourneyType = un.JournyType, IsSelected = false });
                }
                #endregion

                MarkupViewModel vm = new MarkupViewModel
                {
                    Markup = new ApplyMarkup { IsPercentage = false },
                    SelectedGDS = gdstype,
                    SelectedFareType = faretype,
                    SelectedSource = gdsSource,
                    SelectedDayName = gdsDay,
                    SelectedJournyType = gdsj

                };
                model.Add(vm);

                

                return View(model[0]);
            }
            catch
            {

            }
            return View();
        }

        // POST: ApplyMarkups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarkupViewModel model)
        {
            try
            {
                if (model.Markup.AdultMarkup == 0)
                {
                    return NotFound();
                }


                try
                {
                    ApplyMarkup markup = new ApplyMarkup();
                    markup = model.Markup;
                    markup.IsActive = true;
                    markup.CreatedOn = DateTime.UtcNow;
                    _context.Markups.AddAsync(markup);
                    await _context.SaveChangesAsync();
                    #region FAre Type

                    var newMarkupFareType = new List<MarkupFareType>();
                    for (int i = 0; i < model.SelectedFareType.Where(e => e.IsSelected == true).ToList().Count; i++)
                    {
                        newMarkupFareType.Add(new MarkupFareType
                        {
                            Markup = await _context.Markups.Where(e => e.MarkupId == markup.MarkupId).FirstOrDefaultAsync(),
                            FareType = await _context.FareTypes.Where(e => e.FareTypeId == model.SelectedFareType[i].FareTypeId).FirstOrDefaultAsync()

                        });
                    }
                    if (newMarkupFareType.Count() > 0)
                    {
                        _context.MarkupFareTypes.AddRange(newMarkupFareType);
                        await _context.SaveChangesAsync();
                    }

                    #endregion


                    #region Gds

                    var newMarkupGds = new List<MarkupGDS>();
                    for (int i = 0; i < model.SelectedGDS.Where(e => e.IsSelected == true).ToList().Count; i++)
                    {
                        newMarkupGds.Add(new MarkupGDS
                        {
                            Markup = await _context.Markups.Where(e => e.MarkupId == markup.MarkupId).FirstOrDefaultAsync(),
                            gds = await _context.GDS.Where(e => e.GdsId == model.SelectedGDS[i].GdsId).FirstOrDefaultAsync()
                        });
                    }
                    if (newMarkupGds.Count() > 0)
                    {

                        _context.MarkupGds.AddRange(newMarkupGds);
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region Source

                    var newMarkupsource = new List<MarkupMarketingSource>();
                    for (int i = 0; i < model.SelectedSource.Where(e => e.IsSelected == true).ToList().Count; i++)
                    {
                        newMarkupsource.Add(new MarkupMarketingSource
                        {
                            Markup = await _context.Markups.Where(e => e.MarkupId == markup.MarkupId).FirstOrDefaultAsync(),
                            Source = await _context.MarketingSources.Where(e => e.SourceId == model.SelectedSource[i].SourceId).FirstOrDefaultAsync()
                        });
                    }
                    if (newMarkupsource.Count() > 0)
                    {
                        _context.MarkupMarketingSources.AddRange(newMarkupsource);
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region Day Name

                    var newMarkupDay = new List<MarkupDay>();
                    for (int i = 0; i < model.SelectedDayName.Where(e => e.IsSelected == true).ToList().Count; i++)
                    {
                        newMarkupDay.Add(new MarkupDay
                        {
                            Markup = await _context.Markups.Where(e => e.MarkupId == markup.MarkupId).FirstOrDefaultAsync(),
                            Day = await _context.DayName.Where(e => e.DayId == model.SelectedDayName[i].DayNameId).FirstOrDefaultAsync()
                        });
                    }
                    if (newMarkupDay.Count() > 0)
                    {
                        _context.MarkupDay.AddRange(newMarkupDay);
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region Journy Type

                    var newMarkupJourny = new List<MarkupJournyType>();
                    for (int i = 0; i < model.SelectedJournyType.Where(e => e.IsSelected == true).ToList().Count; i++)
                    {
                        newMarkupJourny.Add(new MarkupJournyType
                        {
                            Markup = await _context.Markups.Where(e => e.MarkupId == markup.MarkupId).FirstOrDefaultAsync(),
                            Journy = await _context.JourneyTypes.Where(e => e.JournyTypeId == model.SelectedJournyType[i].JourneyTypeId).FirstOrDefaultAsync()
                        });
                    }
                    if (newMarkupDay.Count > 0)
                    {

                        _context.MarkupJournyTypes.AddRange(newMarkupJourny);
                        await _context.SaveChangesAsync();
                    }


                    #endregion

                    #region Markup cache clear
                    try
                    {
                        string apiUrl = "https://flightreservationjays.azurewebsites.net/api/availability/clearCache";
                        var response = await _httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            var data = await response.Content.ReadAsStringAsync();                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message.ToString());
                    }

                    #endregion
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplyMarkupExists(model.Markup.MarkupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ApplyMarkups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await _context.Markups.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            List<MarkupViewModel> model = new List<MarkupViewModel>();

            #region Working For Fare Type
            List<int> associatedFareTypeIds = _context.MarkupFareTypes
                .Where(mf => mf.Markup.MarkupId == item.MarkupId)
                    .Select(mf => mf.FareType.FareTypeId)
                    .ToList();

            var faretype = await _context.MarkupFareTypes.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.FareType).Select(

                    mg => new MarkupFareTypeViewModel
                    {
                        MarkupId = mg.Markup.MarkupId,
                        FareTypeId = mg.FareType.FareTypeId,
                        FareTypeName = mg.FareType.Fare_Type,
                        IsSelected = true
                    }).ToListAsync();

            var unassociatedfaretype = _context.FareTypes.Where(gds => !associatedFareTypeIds.Contains(gds.FareTypeId)).ToList();
            foreach (var un in unassociatedfaretype)
            {
                faretype.Add(new MarkupFareTypeViewModel { FareTypeId = un.FareTypeId, FareTypeName = un.Fare_Type, IsSelected = false, MarkupId = 0 });
            }
            #endregion

            #region GDS
            List<int> gdsIds = _context.MarkupGds
                .Where(mf => mf.Markup.MarkupId == item.MarkupId)
                .Select(mf => mf.gds.GdsId)
                .ToList();
            var gdstype = await _context.MarkupGds.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.gds).Select(

                   mg => new MarkupGdsViewModel
                   {
                       MarkupId = mg.Markup.MarkupId,
                       GdsId = mg.gds.GdsId,
                       GdsName = mg.gds.GdsName,
                       IsSelected = true
                   }).ToListAsync();

            var unassociatedGds = _context.GDS.Where(gds => !gdsIds.Contains(gds.GdsId)).ToList();
            foreach (var un in unassociatedGds)
            {
                gdstype.Add(new MarkupGdsViewModel { GdsId = un.GdsId, GdsName = un.GdsName, IsSelected = false, MarkupId = 0 });
            }
            #endregion

            #region Source
            List<int> SoruceIds = _context.MarkupMarketingSources
                .Where(mf => mf.Markup.MarkupId == item.MarkupId)
                .Select(mf => mf.Source.SourceId)
                .ToList();
            var gdsSource = await _context.MarkupMarketingSources.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Source).Select(

                   mg => new MarkupAdvertiserViewModel
                   {
                       MarkupId = mg.Markup.MarkupId,
                       SourceId = mg.Source.SourceId,
                       SourceName = mg.Source.SourceName,
                       IsSelected = true
                   }).ToListAsync();

            var unassociatedSource = _context.MarketingSources.Where(gds => !SoruceIds.Contains(gds.SourceId)).ToList();
            foreach (var un in unassociatedSource)
            {
                gdsSource.Add(new MarkupAdvertiserViewModel { SourceId = un.SourceId, MarkupId = 0, SourceName = un.SourceName, IsSelected = false });
            }
            #endregion

            #region Days
            List<int> DayIds = _context.MarkupDay
.Where(mf => mf.Markup.MarkupId == item.MarkupId) // Filter by specific MarkupId
.Select(mf => mf.Day.DayId)
.ToList();
            var gdsDay = await _context.MarkupDay.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Day).Select(

                   mg => new MarkupDayNameViewModel
                   {
                       MarkupId = mg.Markup.MarkupId,
                       DayNameId = mg.Day.DayId,
                       DayName = mg.Day.Day_Name,
                       IsSelected = true
                   }).ToListAsync();

            var unassociatedDay = _context.DayName.Where(gds => !DayIds.Contains(gds.DayId)).ToList();
            foreach (var un in unassociatedDay)
            {
                gdsDay.Add(new MarkupDayNameViewModel { DayNameId = un.DayId, MarkupId = 0, DayName = un.Day_Name, IsSelected = false });
            }
            #endregion

            #region Journy Type
            List<int> Journytypeids = _context.MarkupJournyTypes
                .Where(mf => mf.Markup.MarkupId == item.MarkupId)
                .Select(mf => mf.Journy.JournyTypeId)
                .ToList();
            var jtyp = await _context.MarkupJournyTypes.ToListAsync();
            var gdsj = await _context.MarkupJournyTypes.Where(e => e.Markup.MarkupId == item.MarkupId).Include(mg => mg.Markup).Include(mg => mg.Journy).Select(

               mg => new MarkupJourneyTypeViewModel
               {
                   MarkupId = mg.Markup.MarkupId,
                   JourneyTypeId = mg.Journy.JournyTypeId,
                   JourneyType = mg.Journy.JournyType,
                   IsSelected = true
               }).ToListAsync();

            var unassociatedJ = _context.JourneyTypes.Where(gds => !Journytypeids.Contains(gds.JournyTypeId)).ToList();
            foreach (var un in unassociatedJ)
            {
                gdsj.Add(new MarkupJourneyTypeViewModel { JourneyTypeId = un.JournyTypeId, MarkupId = 0, JourneyType = un.JournyType, IsSelected = false });
            }
            #endregion

            MarkupViewModel vm = new MarkupViewModel
            {
                Markup = item,
                SelectedGDS = gdstype,
                SelectedFareType = faretype,
                SelectedSource = gdsSource,
                SelectedDayName = gdsDay,
                SelectedJournyType = gdsj

            };
            model.Add(vm);


            return View(model[0]);
        }

        // POST: ApplyMarkups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //   public async Task<IActionResult> Edit(MarkupViewModel model ,int id, [Bind("MarkupId,AdultMarkup,ChildMarkup,InfantMarkup,IsPercentage,Airline,BetweenHoursFrom,BetweenHoursTo,StartAirport,EndAirport,FromDate,ToDate,IsActive,CreatedOn")] ApplyMarkup applyMarkup)
        public async Task<IActionResult> Edit(MarkupViewModel model, int id)
        {
            if (id != model.Markup.MarkupId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(model.Markup);
                await _context.SaveChangesAsync();
                var _markup = await _context.Markups.Where(e => e.MarkupId == id).FirstOrDefaultAsync();
                #region FAre Type
                var faretype = await _context.MarkupFareTypes.Where(e => e.Markup.MarkupId == id).ToListAsync();
                if (faretype != null && faretype.Any())
                {
                    _context.MarkupFareTypes.RemoveRange(faretype);
                    await _context.SaveChangesAsync();
                }
                var newMarkupFareType = new List<MarkupFareType>();
                for (int i = 0; i < model.SelectedFareType.Where(e => e.IsSelected == true).ToList().Count; i++)
                {
                    newMarkupFareType.Add(new MarkupFareType
                    {
                        Markup = _markup,
                        FareType = await _context.FareTypes.Where(e => e.FareTypeId == model.SelectedFareType[i].FareTypeId).FirstOrDefaultAsync()
                    });
                }
                _context.MarkupFareTypes.AddRange(newMarkupFareType);
                await _context.SaveChangesAsync();
                #endregion


                #region Gds
                var gdstype = await _context.MarkupGds.Where(e => e.Markup.MarkupId == id).ToListAsync();
                if (gdstype != null && gdstype.Any())
                {
                    _context.MarkupGds.RemoveRange(gdstype);
                    await _context.SaveChangesAsync();
                }

                var newMarkupGds = new List<MarkupGDS>();
                for (int i = 0; i < model.SelectedGDS.Where(e => e.IsSelected == true).ToList().Count; i++)
                {
                    newMarkupGds.Add(new MarkupGDS
                    {
                        Markup = _markup,
                        gds = await _context.GDS.Where(e => e.GdsId == model.SelectedGDS[i].GdsId).FirstOrDefaultAsync()
                    });
                }
                _context.MarkupGds.AddRange(newMarkupGds);
                await _context.SaveChangesAsync();
                #endregion

                #region Source
                var Sourcetype = await _context.MarkupMarketingSources.Where(e => e.Markup.MarkupId == id).ToListAsync();
                if (Sourcetype != null && Sourcetype.Any())
                {
                    _context.MarkupMarketingSources.RemoveRange(Sourcetype);
                    await _context.SaveChangesAsync();
                }

                var newMarkupsource = new List<MarkupMarketingSource>();
                for (int i = 0; i < model.SelectedSource.Where(e => e.IsSelected == true).ToList().Count; i++)
                {
                    newMarkupsource.Add(new MarkupMarketingSource
                    {
                        Markup = _markup,
                        Source = await _context.MarketingSources.Where(e => e.SourceId == model.SelectedSource[i].SourceId).FirstOrDefaultAsync()
                    });
                }
                _context.MarkupMarketingSources.AddRange(newMarkupsource);
                await _context.SaveChangesAsync();
                #endregion

                #region Day Name
                var dayname = await _context.MarkupDay.Where(e => e.Markup.MarkupId == id).ToListAsync();
                if (dayname != null && dayname.Any())
                {
                    _context.MarkupDay.RemoveRange(dayname);
                    await _context.SaveChangesAsync();
                }

                var newMarkupDay = new List<MarkupDay>();
                for (int i = 0; i < model.SelectedDayName.Where(e => e.IsSelected == true).ToList().Count; i++)
                {
                    newMarkupDay.Add(new MarkupDay
                    {
                        Markup = _markup,
                        Day = await _context.DayName.Where(e => e.DayId == model.SelectedDayName[i].DayNameId).FirstOrDefaultAsync()
                    });
                }
                _context.MarkupDay.AddRange(newMarkupDay);
                await _context.SaveChangesAsync();
                #endregion

                #region Journy Type
                var journytype = await _context.MarkupJournyTypes.Where(e => e.Markup.MarkupId == id).ToListAsync();
                if (journytype != null && journytype.Any())
                {
                    _context.MarkupJournyTypes.RemoveRange(journytype);
                    await _context.SaveChangesAsync();
                }

                var newMarkupJourny = new List<MarkupJournyType>();
                for (int i = 0; i < model.SelectedJournyType.Where(e => e.IsSelected == true).ToList().Count; i++)
                {
                    newMarkupJourny.Add(new MarkupJournyType
                    {
                        Markup = _markup,
                        Journy = await _context.JourneyTypes.Where(e => e.JournyTypeId == model.SelectedJournyType[i].JourneyTypeId).FirstOrDefaultAsync()

                    });
                }
                _context.MarkupJournyTypes.AddRange(newMarkupJourny);
                await _context.SaveChangesAsync();
                #endregion

                #region Markup cache clear
                try
                {
                    string apiUrl = "https://flightreservationjays.azurewebsites.net/api/availability/clearCache";
                    var response = await _httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message.ToString());
                }

                #endregion
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplyMarkupExists(model.Markup.MarkupId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            return View(model);
        }

        // GET: ApplyMarkups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applyMarkup = await _context.Markups
                .FirstOrDefaultAsync(m => m.MarkupId == id);
            if (applyMarkup == null)
            {
                return NotFound();
            }

            return View(applyMarkup);
        }

        // POST: ApplyMarkups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applyMarkup = await _context.Markups.FindAsync(id);
            if (applyMarkup != null)
            {
                applyMarkup.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplyMarkupExists(int id)
        {
            return _context.Markups.Any(e => e.MarkupId == id);
        }
    }
}
