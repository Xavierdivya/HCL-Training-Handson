import { Directive, HostBinding, HostListener } from '@angular/core';

@Directive({
  selector: '[appEmailHover]',
  standalone: true
})
export class EmailHoverDirective {

  @HostBinding('style.backgroundColor')
  backgroundColor = 'transparent';

  @HostBinding('style.color')
  textColor = 'black';

  @HostListener('mouseenter')
  onMouseEnter() {
    this.backgroundColor = '#D8F3DC';
    this.textColor = '#14532D';
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.backgroundColor = 'transparent';
    this.textColor = 'black';
  }
}