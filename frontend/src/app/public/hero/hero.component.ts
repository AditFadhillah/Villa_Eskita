import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService } from '../../core/services/language.service';

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.scss'
})
export class HeroComponent implements OnInit {
  translations: any = {};
  currentLanguage: any;

  villaData = {
    backgroundImage: '/images/gallery/logo2.jpg',
    maxGuests: 10,
    pricing: {
      weekday: { eur: 70, idr: 1100000 },
      weekend: { eur: 80, idr: 1260000 }
    },
    facilities: [
      { icon: '🛏️', label: '3 Bedrooms' },
      { icon: '🚿', label: '2 Bathrooms' },
      { icon: '🍳', label: '1 Kitchen' },
      { icon: '🛋️', label: 'Big Living Room' },
      { icon: '📶', label: 'Wi-Fi' },
      { icon: '📺', label: 'Smart TV' },
      { icon: '🚿', label: 'Water Heater' },
      { icon: '🎤', label: 'Karaoke' }
    ]
  };

  constructor(private languageService: LanguageService) {}

  ngOnInit() {
    this.currentLanguage = this.languageService.getLanguage();
    this.updateTranslations();
    this.languageService.language$.subscribe(() => {
      this.currentLanguage = this.languageService.getLanguage();
      this.updateTranslations();
    });
  }

  updateTranslations() {
    this.translations = this.languageService.getCurrentTranslations();
  }
}
