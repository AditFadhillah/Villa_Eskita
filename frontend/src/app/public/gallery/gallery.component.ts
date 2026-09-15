import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService } from '../../core/services/language.service';

@Component({
  selector: 'app-gallery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.scss'
})
export class GalleryComponent implements OnInit {
  translations: any = {};

  galleryImages = [
    {
      src: '/images/villa/facade.png',
      title: 'Villa Exterior',
      description: 'Stunning exterior view of Villa Eskita with lush greenery'
    },
    {
      src: '/images/villa/livingroom1.1.jpeg',
      title: 'Living Room',
      description: 'Spacious and comfortable living area'
    },
    {
      src: '/images/villa/diningroom1.jpeg',
      title: 'Dining Room',
      description: 'Elegant dining space for family gatherings'
    },
    {
      src: '/images/villa/bedroom1.jpeg',
      title: 'Master Bedroom',
      description: 'Comfortable master bedroom with modern amenities'
    },
    {
      src: '/images/villa/bedroom2.jpeg',
      title: 'Guest Bedroom',
      description: 'Spacious guest bedroom'
    },
    {
      src: '/images/villa/livingroom1.2.jpeg',
      title: 'Living Area Detail',
      description: 'Cozy corner of the living area'
    }
  ];

  selectedImage: any = null;

  constructor(private languageService: LanguageService) {}

  ngOnInit() {
    this.updateTranslations();
    this.languageService.language$.subscribe(() => {
      this.updateTranslations();
    });
  }

  updateTranslations() {
    this.translations = this.languageService.getCurrentTranslations();
  }

  openLightbox(image: any) {
    this.selectedImage = image;
  }

  closeLightbox() {
    this.selectedImage = null;
  }
}
