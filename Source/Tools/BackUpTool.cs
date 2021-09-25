﻿using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Ionic.Zip;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.Tools
{
    public class BackupTool : ITool
    {
        private readonly IMailboxService _mailboxService;
        private readonly IPathManager _pathManager;

        public BackupTool(IPathManager pathManager, IMailboxService mailboxService)
        {
            _pathManager = pathManager;
            _mailboxService = mailboxService;
        }

        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/backup.png")));
            }
        }

        public ScriptInfo Info => new("backup", "Backup", "Internal", "Backup all your files");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
            _mailboxService.PostAction(async () =>
            {
                NotificationService.Show("Start Compressing");

                using (var zip = new ZipFile())
                {
                    NotificationService.Show("Compressing");
                    zip.AddDirectory(Path.Combine(_pathManager.ConfigBaseDir, "Notebooks"), "Notebooks");
                    zip.AddDirectory(Path.Combine(_pathManager.ConfigBaseDir, "Templates"), "Templates");
                    zip.AddDirectory(Path.Combine(_pathManager.ConfigBaseDir, "Scripts"), "Scripts");
                    zip.AddDirectory(Path.Combine(_pathManager.ConfigBaseDir, "Screens"), "Screens");

                    zip.Comment = "This backup was generated by Slithin";

                    zip.Save(Path.Combine(_pathManager.BackupsDir, $"Backup_from_{DateTime.Now:yyyy-dd-M--HH-mm-ss}" + ".zip"));
                }

                NotificationService.Show("Finished");
                await Task.Delay(1000);

                NotificationService.Hide();
            });
        }
    }
}
