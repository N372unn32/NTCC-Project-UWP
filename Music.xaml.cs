using College_Project_Version_One.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.Media.Audio;
using Windows.Media.Playback;
using System.Linq;
using Windows.UI.Xaml.Markup;
using Windows.Media.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Search;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Music : Page
    {
        private ObservableCollection<Song> Songs = new ObservableCollection<Song>();
        private ObservableCollection<StorageFile> AllSongs = new ObservableCollection<StorageFile>();
        public MediaSource _mediaSource;
        public MediaPlayer _mediaPlayer;
    //    bool _playingMusic = false;
        
        public Music()
        {
            this.InitializeComponent();
        }

        private void SongsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = false;
           
        }

     //   [Obsolete]//Attribute("Obsolete Function used")
        private  void  SongsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var s = e.ClickedItem as StorageFile;
            var clickedSong = (Song)e.ClickedItem;
            //MyMediaPlayer.Source = clickedSong.SongFile.OpenReadAsync();
            //MyMediaPlayer.Source=(await clickedSong.SongFile.OpenAsync(FileAccessMode.Read), clickedSong.SongFile.ContentType);
            //MyMediaElement.SetSource(await song.SongFile.OpenAsync(FileAccessMode.Read), song.SongFile.ContentType);

            foreach (var item in Songs)
            {
                if (clickedSong.Title==item.Title)
                {
                    //MyMediaPlayer.MediaPlayer.SetStreamSource(await item.SongFile.OpenAsync(FileAccessMode.Read));
                    //MyMediaPlayer.MediaPlayer.source

                    _mediaSource = MediaSource.CreateFromStorageFile(item.SongFile);
                    _mediaPlayer = new MediaPlayer();
                    _mediaPlayer.Source = _mediaSource;
                    MyMediaPlayer.SetMediaPlayer(_mediaPlayer);

                    //  MyMediaPlayer.MediaPlayer.SetFileSource((IStorageFile)item.SongFile);

                    //          MyMediaPlayer.MediaPlayer.SetFileSource(await item.SongFile.OpenAsync(FileAccessMode.Read), item.SongFile.ContentType);
                    //      MyMediaPlayer.MediaPlayer.SetStreamSource(await item.SongFile.OpenAsync(FileAccessMode.Read));
                    //    MyMediaPlayer.MediaPlayer.SetFileSource(item.SongFile);
                    //    MyMediaPlayer.MediaPlayer.SetStreamSource((await clickedSong.SongFile.OpenAsync(FileAccessMode.Read), clickedSong.SongFile.ContentType));
                    // MyMediaPlayer.Source =await item.SongFile.OpenReadAsync();

                    //===============================Set Media Background later
                    //var brush = new SolidColorBrush();
                    //= item.AlbumCover.UriSource;
                    //MyMediaPlayer.MediaPlayer.= item.AlbumCover;
                    MyMediaPlayer.PosterSource = item.AlbumCover;
                    MyMediaPlayer.MediaPlayer.Play();
                }
            }
        }

        private async Task RetrieveFilesInFolders(ObservableCollection<StorageFile> list,StorageFolder parent)
        {
            foreach (var item in await parent.GetFilesAsync())
            {
                if (item.FileType == ".mp3")
                {
                   list.Add(item);
                }
            }
            foreach (var item in await parent.GetFoldersAsync())
            {
                await RetrieveFilesInFolders(list, item);
            }

        }
        

        private async Task<ObservableCollection<StorageFile>> SetupMusicList()
        {
            var s = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
            StorageFolder folder = KnownFolders.MusicLibrary;
            var allSongs = new ObservableCollection<StorageFile>();
            await RetrieveFilesInFolders(allSongs, folder);
            return allSongs;
        }

        private async Task<List<StorageFile>> PopulateSongs(ObservableCollection<StorageFile> allsongs)
        {
            bool IsDuplicate = true;
            var tempList = new List<StorageFile>();
            foreach (var song in allsongs)
            {
                MusicProperties musicProperties = await song.Properties.GetMusicPropertiesAsync();
                foreach (var tempSong in tempList)
                {
                    var tempProperties = await tempSong.Properties.GetMusicPropertiesAsync();
                    if ((!string.IsNullOrEmpty(musicProperties.Title))&&(musicProperties.Title != tempProperties.Title))
                    {
                        IsDuplicate = false;
                    }
                    else IsDuplicate = true;
                }
                if (!IsDuplicate)
                {
                    tempList.Add(song);
                }
            }
            return tempList;
        }
        private async Task PopulateNonDuplicateSongs(List<StorageFile> Files)
        {
            int id = 0;
            foreach (var File in Files)
            {
                MusicProperties songProperties = await File.Properties.GetMusicPropertiesAsync();
                StorageItemThumbnail currentThumb = await File.GetThumbnailAsync(ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);

                var song = new Song();
                song.Id = id;
                song.Title = songProperties.Title;
                song.Artist = songProperties.Artist;
                song.Album = songProperties.Album;
                var albumCover = new BitmapImage();
                albumCover.SetSource(currentThumb);
                song.AlbumCover = albumCover;
                song.SongFile = File;
                Songs.Add(song);
                id++;


            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            //  AllSongs = await SetupMusicList();
            //var NonDuplicateSongs=  await PopulateSongs(AllSongs);
            //  await PopulateNonDuplicateSongs(NonDuplicateSongs);
            QueryOptions queryOption = new QueryOptions
          (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".wma" });
            
            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();
            int id = 0;
            foreach (var file in files)
            {
                var MusicFile = new Song();

                var fileProperties = await file.Properties.GetMusicPropertiesAsync();
                StorageItemThumbnail fileImage =await file.GetThumbnailAsync(ThumbnailMode.MusicView,200,ThumbnailOptions.UseCurrentScale);
                var cover = new BitmapImage();
                cover.SetSource(fileImage);
                MusicFile.Album=fileProperties.Album;
                MusicFile.Artist=fileProperties.Artist;
                MusicFile.Id=id;
                MusicFile.SongFile = file;
                MusicFile.Title=fileProperties.Title;
                
                MusicFile.AlbumCover=cover;
                Songs.Add(MusicFile);
                // do something with the music files
                id++;
            }
            MyProgressRing.IsActive = false;
        }
    }
}
