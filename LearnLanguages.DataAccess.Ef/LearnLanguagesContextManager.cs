﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Csla.Data;

namespace LearnLanguages.DataAccess.Ef
{
  public sealed class LearnLanguagesContextManager
  {
    private LearnLanguagesContextManager()
    {
      Initialize();
    }

    private static object _InstanceLock = new object();
    private static volatile LearnLanguagesContextManager _Instance;

    public static LearnLanguagesContextManager Instance
    {
      get
      {
        if (_Instance == null)
        {
          lock (_InstanceLock)
          {
            if (_Instance == null)
            {
              _Instance = new LearnLanguagesContextManager();
            }
          }
        }

        return _Instance;
      }
    }

    private void Initialize()
    {
      var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
      var isBackground = System.Threading.Thread.CurrentThread.IsBackground;
      var isPool = System.Threading.Thread.CurrentThread.IsThreadPoolThread;
      using (LearnLanguagesContext context = new LearnLanguagesContext())
      {
        if (context.DatabaseExists() && bool.Parse(EfResources.DeleteAllExistingDataAndStartNewSeedData))
          context.DeleteDatabase();

        if (!context.DatabaseExists())
        {
          context.CreateDatabase();
          context.Connection.Open();
          SeedContext(context);
          context.SaveChanges();
        }
      }
    }

    private void SeedContext(LearnLanguagesContext context)
    {
      //LANGUAGES
      foreach (var langDto in SeedData.Instance.Languages)
      {
        var langData = EfHelper.ToData(langDto);
        context.LanguageDatas.AddObject(langData);
        context.SaveChanges();

        //UPDATE SEED DATA PHRASES WITH NEW LANGUAGE ID
        var affectedPhrases = (from phraseDto in SeedData.Instance.Phrases
                               where phraseDto.LanguageId == langDto.Id
                               select phraseDto).ToList();
        foreach (var phraseDto in affectedPhrases)
        {
          phraseDto.LanguageId = langData.Id;//new Id
        }
        langDto.Id = langData.Id;
      }

      //ROLES
      foreach (var roleDto in SeedData.Instance.Roles)
      {
        var roleData = EfHelper.ToData(roleDto);
        context.RoleDatas.AddObject(roleData);
        context.SaveChanges();

        //UPDATE SEED DATA THAT REFERENCES THE ID OF THIS DATA
        var affectedUsers = (from userDto in SeedData.Instance.Users
                             where userDto.RoleIds.Contains(roleDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.RoleIds.Remove(roleDto.Id);
          affectedUser.RoleIds.Add(roleData.Id);
        }
      }

      ////USERS AND PHRASES PER USER
      //foreach (var userDto in SeedData.Instance.Users)
      //{

      //}
      
      
      
      
      foreach (var userDto in SeedData.Instance.Users)
      {
        var userData = EfHelper.ToData(userDto, false);
        context.UserDatas.AddObject(userData);
        context.SaveChanges();

        //UPDATE SEED DATA THAT REFERENCES THE ID OF THIS USER
        var affectedPhrases = (from phraseDto in SeedData.Instance.Phrases
                               where phraseDto.UserId == userDto.Id
                               select phraseDto).ToList();

        foreach (var affectedPhrase in affectedPhrases)
        {
          affectedPhrase.UserId = userData.Id;
          affectedPhrase.UserId = userData.Id;
        }

        userDto.Id = userData.Id;
      }

      //PHRASES
      foreach (var phraseDto in SeedData.Instance.Phrases)
      {
        var phraseData = EfHelper.ToData(phraseDto);
        context.PhraseDatas.AddObject(phraseData);
        context.SaveChanges();

        //UPDATE SEED DATA THAT REFERENCES THE ID OF THIS DATA
        var affectedUsers = (from userDto in SeedData.Instance.Users
                             where userDto.PhraseIds.Contains(phraseDto.Id)
                             select userDto).ToList();

        foreach (var affectedUser in affectedUsers)
        {
          affectedUser.PhraseIds.Remove(phraseDto.Id);
          affectedUser.PhraseIds.Add(phraseData.Id);
        }

        phraseDto.Id = phraseData.Id;
      }
    }

    public ObjectContextManager<LearnLanguagesContext> GetManager()
    {
      return ObjectContextManager<LearnLanguagesContext>.GetManager(EfResources.LearnLanguagesConnectionStringKey);
    }
  }
}
