import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

export type Language = 'en' | 'id';

export const translations = {
  en: {
    nav: {
      home: 'Home',
      gallery: 'Gallery',
      contact: 'Contact'
    },
    hero: {
      title: 'Villa Eskita Anyer',
      subtitle: 'Luxury Villa Retreat in Paradise',
      description: 'Experience the ultimate tropical getaway with stunning ocean views, private amenities, and world-class comfort.',
      maxGuests: 'Max Guests',
      perNight: 'Per Night'
    },
    facilities: {
      title: 'Villa Facilities',
      bedrooms: '3 Bedrooms',
      bathrooms: '2 Bathrooms',
      kitchen: '1 Kitchen',
      livingRoom: 'Big Living Room',
      wifi: 'Wi-Fi',
      smartTv: 'Smart TV',
      waterHeater: 'Water Heater',
      karaoke: 'Karaoke',
      maxGuests: 'Max 10 Guests'
    },
    attractions: {
      title: 'Local Attractions',
      beach: '5 minutes walk to the beach',
      restaurants: 'Local restaurants & bars',
      hotSprings: 'Natural hot springs',
      volcano: 'Trip to Krakatoa Volcano',
      waterfalls: 'Waterfalls & water sports',
      karangBolong: 'Karang Bolong'
    },
    pricing: {
      title: 'Daily Rates',
      weekday: 'Sunday - Thursday',
      weekend: 'Friday - Saturday',
      currency: {
        eur: 'EUR',
        idr: 'IDR'
      }
    },
    contact: {
      title: 'Get in Touch',
      subtitle: 'Have questions? We\'d love to hear from you',
      location: 'Location',
      phone: 'Phone',
      email: 'Email',
      hours: 'Availability',
      nameLabel: 'Full Name',
      namePlaceholder: 'Your Full Name',
      emailLabel: 'Email Address',
      emailPlaceholder: 'your@email.com',
      phoneLabel: 'Phone Number',
      phonePlaceholder: '+62 821 1234 5678',
      messageLabel: 'Message',
      messagePlaceholder: 'Tell us about your inquiry...',
      submit: 'Send Message',
      sending: 'Sending...',
      successMessage: 'Thank you! We received your message. We\'ll get back to you soon.'
    },
    gallery: {
      title: 'Photo Gallery',
      subtitle: 'Explore the beauty of our villa'
    },
    footer: 'All rights reserved'
  },
  id: {
    nav: {
      home: 'Beranda',
      gallery: 'Galeri',
      contact: 'Kontak'
    },
    hero: {
      title: 'Villa Eskita Anyer',
      subtitle: 'Resor Mewah di Surga Tropis',
      description: 'Nikmati pengalaman liburan tropis terbaik dengan pemandangan pantai yang menakjubkan, fasilitas pribadi, dan kenyamanan kelas dunia.',
      maxGuests: 'Kapasitas Maksimal',
      perNight: 'Per Malam'
    },
    facilities: {
      title: 'Fasilitas Villa',
      bedrooms: '3 Kamar Tidur',
      bathrooms: '2 Kamar Mandi',
      kitchen: '1 Dapur',
      livingRoom: 'Ruang Keluarga Besar',
      wifi: 'Wi-Fi',
      smartTv: 'Smart TV',
      waterHeater: 'Pemanas Air',
      karaoke: 'Karaoke',
      maxGuests: 'Maksimal 10 Tamu'
    },
    attractions: {
      title: 'Atraksi Lokal',
      beach: 'Berjalan 5 menit ke pantai',
      restaurants: 'Restoran dan bar lokal',
      hotSprings: 'Mata air panas alami',
      volcano: 'Perjalanan ke Gunung Krakatau',
      waterfalls: 'Air terjun dan olahraga air',
      karangBolong: 'Karang Bolong'
    },
    pricing: {
      title: 'Harga Harian',
      weekday: 'Minggu - Kamis',
      weekend: 'Jumat - Sabtu',
      currency: {
        eur: 'EUR',
        idr: 'IDR'
      }
    },
    contact: {
      title: 'Hubungi Kami',
      subtitle: 'Ada pertanyaan? Kami ingin mendengar dari Anda',
      location: 'Lokasi',
      phone: 'Telepon',
      email: 'Email',
      hours: 'Ketersediaan',
      nameLabel: 'Nama Lengkap',
      namePlaceholder: 'Nama Lengkap Anda',
      emailLabel: 'Alamat Email',
      emailPlaceholder: 'anda@email.com',
      phoneLabel: 'Nomor Telepon',
      phonePlaceholder: '+62 821 1234 5678',
      messageLabel: 'Pesan',
      messagePlaceholder: 'Ceritakan pertanyaan Anda...',
      submit: 'Kirim Pesan',
      sending: 'Mengirim...',
      successMessage: 'Terima kasih! Kami menerima pesan Anda. Kami akan segera menghubungi Anda kembali.'
    },
    gallery: {
      title: 'Galeri Foto',
      subtitle: 'Jelajahi keindahan villa kami'
    },
    footer: 'Hak cipta dilindungi'
  }
};

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private languageSubject = new BehaviorSubject<Language>('en');
  public language$ = this.languageSubject.asObservable();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    // Only access localStorage in browser environment
    if (isPlatformBrowser(platformId) && typeof localStorage !== 'undefined') {
      try {
        const savedLanguage = localStorage.getItem('language') as Language | null;
        if (savedLanguage && (savedLanguage === 'en' || savedLanguage === 'id')) {
          this.languageSubject.next(savedLanguage);
        }
      } catch (e) {
        // localStorage not available, use default
      }
    }
  }

  setLanguage(lang: Language) {
    this.languageSubject.next(lang);
    if (isPlatformBrowser(this.platformId) && typeof localStorage !== 'undefined') {
      try {
        localStorage.setItem('language', lang);
      } catch (e) {
        // localStorage not available
      }
    }
  }

  getLanguage(): Language {
    return this.languageSubject.value;
  }

  getTranslations(lang: Language) {
    return translations[lang];
  }

  getCurrentTranslations() {
    return translations[this.getLanguage()];
  }
}
