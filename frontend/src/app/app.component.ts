import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './public/header/header.component';
import { HeroComponent } from './public/hero/hero.component';
import { GalleryComponent } from './public/gallery/gallery.component';
import { ContactComponent } from './public/contact/contact.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HeaderComponent, HeroComponent, GalleryComponent, ContactComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Villa Eskita';
}
