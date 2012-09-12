using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;
using System;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongViewModel : ViewModelBase
  {
    public AddSongViewModel()
    {
      var id = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(id);
      MultiLineTextEdit.NewMultiLineTextEdit((s, r) =>
        {
          History.Events.ThinkedAboutTargetEvent.Publish(id);
          if (r.Error != null)
            throw r.Error;

          var songViewModel = Services.Container.GetExportedValue<AddSongMultiLineTextEditViewModel>();
          songViewModel.Model = r.Object;
          Song = songViewModel;
          Song.Saved += new System.EventHandler<Common.EventArgs.ModelEventArgs<MultiLineTextEdit>>(Song_Saved);
        });
      //PhraseEdit.NewPhraseEdit((s, r) =>
      //  {
      //    if (r.Error != null)
      //      throw r.Error;

      //    var phraseViewModel = Services.Container.GetExportedValue<AddSongMultiLineTextEditViewModel>();
      //    phraseViewModel.Model = r.Object;
      //    Song = phraseViewModel;
      //  });
    }

    public void Song_Saved(object sender, Common.EventArgs.ModelEventArgs<MultiLineTextEdit> e)
    {
      //once the song is saved, we should go to a different screen...view songs maybe?  Save successful screen?
      //edit song viewmodel?
    }

    private AddSongMultiLineTextEditViewModel _Song;
    public AddSongMultiLineTextEditViewModel Song
    {
      get { return _Song; }
      set
      {
        if (value != _Song)
        {
          _Song = value;
          NotifyOfPropertyChange(() => Song);
        }
      }
    }
  }
}
