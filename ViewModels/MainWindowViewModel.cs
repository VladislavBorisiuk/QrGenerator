﻿using BaseClassesLyb;
using Microsoft.Win32;
using QRCoder;
using QrGenerator.infrastructure;
using QrGenerator.Services;
using QrGenerator.Services.infrastructures;
using System;
using System.Windows;
using System.Windows.Media;
using WpfBaseLyb;
using Color = System.Windows.Media.Color;


namespace QrGenerator.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ImageSaver _imageSaver;
        private ImageSource _qrCodeImage;
        private QrCodeGeneratorService _qrCodeGeneratorService;
        private ImageSource _qrCodeIcon;

        public ImageSource QrCodeIcon
        {
            get => _qrCodeIcon;

            set
            {
                _qrCodeIcon = value;
                OnPropertyChanged(nameof(QrCodeIcon));
            }
        }

        private string QrIconPath;

        private Color _backColor;

        public Color BackColor
        {
            get => _backColor;

            set
            {
                _backColor = value;
                OnPropertyChanged(nameof(BackColor));
            }

        }

        private Color _frontColor;

        public Color FrontColor
        {
            get => _frontColor;

            set
            {
                _frontColor = value;
                OnPropertyChanged(nameof(FrontColor));
            }
        }
        public ImageSource QRCodeImage
        {
            get => _qrCodeImage;

            set 
            {
                _qrCodeImage = value;
                OnPropertyChanged(nameof(QRCodeImage));
            }
        }
        public RegularCommand GenerateQrCode { get; }
        private bool CanGenerateQrCodeExecute(object p) => true;

        private void GenerateQrCodeExecute(object str)
        {
            QRCodeImage = _qrCodeGeneratorService.GenerateQrCodeUrl(str.ToString(), BackColor, FrontColor, QrIconPath);
        }

        public SimpleCommand DownLoadIconCode { get; }
        private bool CanDownLoadIconExecute() => true;

        private void DownLoadIconExecute()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFile = openFileDialog.FileName;
                    QrCodeIcon = GlobalConverter.ConvertStringToImageSource(selectedFile);
                    QrIconPath = openFileDialog.FileName;
                }
                else
                {
                    MessageBox.Show("Файлы не выбраны.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }

        }

        public SimpleCommand DownLoadQrCode { get; }
        private bool CanDownLoadQrCodeExecute() => true;

        private void DownLoadQrCodeExecute()
        {
            _imageSaver.SaveImageToFile(QRCodeImage);
        }
        public MainWindowViewModel(IImageSaver imageSaver, IQrCodeGeneratorService qrCodeGeneratorService) 
        {
            _imageSaver = (ImageSaver?)imageSaver;
            _qrCodeGeneratorService = (QrCodeGeneratorService?)qrCodeGeneratorService;

            GenerateQrCode = new RegularCommand(GenerateQrCodeExecute, CanGenerateQrCodeExecute);
            DownLoadIconCode = new SimpleCommand(DownLoadIconExecute, CanDownLoadIconExecute);
            DownLoadQrCode = new SimpleCommand(DownLoadQrCodeExecute, CanDownLoadQrCodeExecute);
        }
    }
}
