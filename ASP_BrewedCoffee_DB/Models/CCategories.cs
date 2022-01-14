﻿namespace ASP_BrewedCoffee_DB.Models;
public class CCategories
{
    private CDBContext DB = CConf.DB;
    public IEnumerable<CCategory>? GetCats()
    {
        return DB.Categories;
    }
    public void Add(CCategory cat)
    {
        DB.Categories.Add(cat);
        DB.SaveChanges();
    }
    public void Edit(CCategory cat, int id)
    {
        var category = DB.Categories.Find(id);
        category.Title = cat.Title;
        category.Slug = cat.Slug;
        DB.SaveChanges();
    }
    public CCategory GetCat(int? id) => DB.Categories.Find(id);
    public static string GetCatName(IEnumerable<CCategory> cats, int id)
    {
        foreach (CCategory cat in cats) if (cat.Id == id) return cat.Title;

        return "";
    }
    public static int GetCatID(CMenu menu, string slug)
    {
        foreach (CMenuItem item in menu) if (item.Slug == slug) return item.Id;

        return -1;
    }
}