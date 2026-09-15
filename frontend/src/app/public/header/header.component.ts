import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService, Language } from '../../core/services/language.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  currentLanguage: Language = 'en';
  languages: Language[] = ['en', 'id'];
  languageLabels = { en: 'English', id: 'Bahasa Indonesia' };

  navItems: any[] = [];

  constructor(private languageService: LanguageService) {}

  ngOnInit() {
    this.currentLanguage = this.languageService.getLanguage();
    this.updateNavItems();
    this.languageService.language$.subscribe(() => {
      this.currentLanguage = this.languageService.getLanguage();
      this.updateNavItems();
    });
  }

  updateNavItems() {
    const trans = this.languageService.getCurrentTranslations();
    this.navItems = [
      { label: trans.nav.home, id: 'home' },
      { label: trans.nav.gallery, id: 'gallery' },
      { label: trans.nav.contact, id: 'contact' }
    ];
  }

  scrollToSection(id: string) {
    const element = document.getElementById(id);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  changeLanguage(lang: Language) {
    this.languageService.setLanguage(lang);
  }
}
