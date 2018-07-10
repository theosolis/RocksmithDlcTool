using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DlcToolLib.Model;
using Shouldly;

namespace DlcToolLib.UnitTests
{
	[TestFixture]
	public class TestOfficialDlcRemapper
	{
		[Test]
		public void WhenOnlySongRemappedArtistStaysTheSame()
		{
			var originalSongName = "dis one";
			var newSongName = "dee other one";
			var artistName = "joe";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistName, Song = originalSongName, NewSong = newSongName });

			
			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistName, Song = originalSongName, SongPack = "blah", Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(artistName);
			remapped.Song.ShouldBe(newSongName);

			officialDlcItem.Artist.ShouldBe(artistName);
			officialDlcItem.Song.ShouldBe(originalSongName);
		}

		[Test]
		public void WhenOnlyArtistRemappedSongStaysTheSame()
		{
			var originalSongName = "dis one";
			var artistName = "joe";
			var newArtistName = "doug";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistName, NewArtist = newArtistName });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistName, Song = originalSongName, SongPack = "blah", Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(newArtistName);
			remapped.Song.ShouldBe(originalSongName);

			officialDlcItem.Artist.ShouldBe(artistName);
			officialDlcItem.Song.ShouldBe(originalSongName);
		}


		[Test]
		public void WhenArtistAndSongRemappedBothChange()
		{
			var originalSongName = "dis one";
			var artistName = "joe";
			var newArtistName = "doug";
			var newSongName = "dee other one";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistName, NewArtist = newArtistName, Song = originalSongName, NewSong = newSongName });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistName, Song = originalSongName, SongPack = "blah", Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(newArtistName);
			remapped.Song.ShouldBe(newSongName);

			officialDlcItem.Artist.ShouldBe(artistName);
			officialDlcItem.Song.ShouldBe(originalSongName);
		}

		[Test]
		public void WhenArtistDoesNotMatchNothingChanges()
		{
			var originalSongName = "dis one";
			var artistNameToRemap = "joe";
			var originalSongPack = "thepack";
			var newArtistName = "doug";
			var newSongName = "dee other one";
			var newSongPack = "ivechanged";
			var artistNameDoesntMatch = "freddy";
			
			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistNameToRemap, NewArtist = newArtistName, Song = originalSongName, NewSong = newSongName, NewSongPack = newSongPack });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistNameDoesntMatch, Song = originalSongName, SongPack = originalSongPack, Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(artistNameDoesntMatch);
			remapped.Song.ShouldBe(originalSongName);
			remapped.SongPack.ShouldBe(originalSongPack);

			officialDlcItem.Artist.ShouldBe(artistNameDoesntMatch);
			officialDlcItem.Song.ShouldBe(originalSongName);
			officialDlcItem.SongPack.ShouldBe(originalSongPack);
		}

		[Test]
		public void IfOnlyArtistAndNoChangesSpecifiedThenNothingChanges()
		{
			var originalSongName = "dis one";
			var artistNameToRemap = "joe";
			var originalSongPack = "thepack";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistNameToRemap });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistNameToRemap, Song = originalSongName, SongPack = originalSongPack, Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(artistNameToRemap);
			remapped.Song.ShouldBe(originalSongName);
			remapped.SongPack.ShouldBe(originalSongPack);

			officialDlcItem.Artist.ShouldBe(artistNameToRemap);
			officialDlcItem.Song.ShouldBe(originalSongName);
			officialDlcItem.SongPack.ShouldBe(originalSongPack);
		}

		[Test]
		public void IfArtistAndSongAndNoChangesSpecifiedThenNothingChanges()
		{
			var originalSongName = "dis one";
			var artistNameToRemap = "joe";
			var originalSongPack = "thepack";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistNameToRemap, Song = originalSongName });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistNameToRemap, Song = originalSongName, SongPack = originalSongPack, Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(artistNameToRemap);
			remapped.Song.ShouldBe(originalSongName);
			remapped.SongPack.ShouldBe(originalSongPack);

			officialDlcItem.Artist.ShouldBe(artistNameToRemap);
			officialDlcItem.Song.ShouldBe(originalSongName);
			officialDlcItem.SongPack.ShouldBe(originalSongPack);
		}

		[Test]
		public void WhenPartialMatchThenNothingChanges()
		{
			var songNameToMatch = "dis one";
			var artistNameToRemap = "joe";
			var originalSongPack = "thepack";
			var newArtistName = "doug";
			var newSongName = "dee other one";
			var newSongPack = "ivechanged";
			var unmatchedSongName = "this doesn't match";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.Entries.Add(new RemapOfficialEntries.Entry { Artist = artistNameToRemap, NewArtist = newArtistName, Song = songNameToMatch, NewSong = newSongName, NewSongPack = newSongPack });


			var mapper = new OfficialDlcRemapper(remapEntries);
			var officialDlcItem = new OfficialDlcItem { Artist = artistNameToRemap, Song = unmatchedSongName, SongPack = originalSongPack, Genre = "hippy", Year = "1969" };
			var remapped = mapper.Remap(officialDlcItem);

			remapped.Artist.ShouldBe(artistNameToRemap);
			remapped.Song.ShouldBe(unmatchedSongName);
			remapped.SongPack.ShouldBe(originalSongPack);

			officialDlcItem.Artist.ShouldBe(artistNameToRemap);
			officialDlcItem.Song.ShouldBe(unmatchedSongName);
			officialDlcItem.SongPack.ShouldBe(originalSongPack);
		}

		[Test]
		public void NoMissingEntriesReturnsEmptyList()
		{
			var remapEntries = new RemapOfficialEntries();

			var mapper = new OfficialDlcRemapper(remapEntries);
			mapper.GetMissingEntries().Count().ShouldBe(0);
		}

		[Test]
		public void SingleMissingEntryReturnsSingleItemInList()
		{
			var songName = "dis one";
			var artistName = "joe";
			var originalSongPack = "thepack";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.AddMissing.Add(
			new RemapOfficialEntries.MissingEntry { Artist = artistName, Song = songName, SongPack = originalSongPack }
			);

			var mapper = new OfficialDlcRemapper(remapEntries);
			var missing = mapper.GetMissingEntries().ToList();
			missing.Count().ShouldBe(1);
			missing[0].Song.ShouldBe(songName);
			missing[0].Artist.ShouldBe(artistName);
			missing[0].SongPack.ShouldBe(originalSongPack);
		}

		[Test]
		public void MultipleMissingEntriesReturnsAllInList()
		{
			var songName1 = "dis one";
			var artistName1 = "joe";
			var originalSongPack1 = "thepack";

			var songName2 = "dat one";
			var artistName2 = "blow";
			var originalSongPack2 = "thepackage";

			var remapEntries = new RemapOfficialEntries();
			remapEntries.AddMissing.Add(
			new RemapOfficialEntries.MissingEntry { Artist = artistName1, Song = songName1, SongPack = originalSongPack1 }
			);
			remapEntries.AddMissing.Add(
			new RemapOfficialEntries.MissingEntry { Artist = artistName2, Song = songName2, SongPack = originalSongPack2 }
			);

			var mapper = new OfficialDlcRemapper(remapEntries);
			var missing = mapper.GetMissingEntries().ToList();
			missing.Count().ShouldBe(2);

			var theFirst = missing.Single(x => x.Artist == artistName1);
			var theSecond = missing.Single(x => x.Artist == artistName2);

			theFirst.Song.ShouldBe(songName1);
			theFirst.Artist.ShouldBe(artistName1);
			theFirst.SongPack.ShouldBe(originalSongPack1);

			theSecond.Song.ShouldBe(songName2);
			theSecond.Artist.ShouldBe(artistName2);
			theSecond.SongPack.ShouldBe(originalSongPack2);
		}
	}
}
