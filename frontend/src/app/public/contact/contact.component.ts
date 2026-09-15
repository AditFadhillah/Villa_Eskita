import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LanguageService } from '../../core/services/language.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent implements OnInit {
  translations: any = {};

  villaContact = {
    address: 'Anyer, Banten 42167, Indonesia',
    phone: '+62 821 1234 5678',
    email: 'info@villaeskita.com',
    hours: '24/7 Available'
  };

  contactForm = {
    name: '',
    email: '',
    phone: '',
    message: ''
  };

  submitted = false;
  submitting = false;
  successMessage = '';
  errorMessage = '';

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

  onSubmit() {
    if (this.isFormValid()) {
      this.submitting = true;
      // Simulate API call
      setTimeout(() => {
        this.submitting = false;
        this.submitted = true;
        this.successMessage = this.translations.contact?.successMessage || 'Thank you! We received your message.';
        this.resetForm();
        
        // Clear message after 5 seconds
        setTimeout(() => {
          this.submitted = false;
        }, 5000);
      }, 1000);
    }
  }

  isFormValid(): boolean {
    return this.contactForm.name.trim() !== '' &&
           this.contactForm.email.trim() !== '' &&
           this.contactForm.phone.trim() !== '' &&
           this.contactForm.message.trim() !== '';
  }

  resetForm() {
    this.contactForm = {
      name: '',
      email: '',
      phone: '',
      message: ''
    };
  }
}
