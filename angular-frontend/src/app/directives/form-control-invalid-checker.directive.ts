import { Directive, HostBinding, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { AbstractControl } from "@angular/forms";
import { Subscription } from "rxjs";

@Directive({
  selector: '[appFormControlInvalidChecker]'
})
export class FormControlInvalidCheckerDirective implements OnInit, OnDestroy {

  @Input("appFormControlInvalidChecker") control: AbstractControl;

  @HostBinding('class.is-invalid') invalidFeedbackClassBinding: boolean = false;

  subscription: Subscription;

  constructor() {
    this.invalidFeedbackClassBinding = false;
  }

  ngOnInit(): void {

    this.subscription = this.control.statusChanges.subscribe(status => {
      this.invalidFeedbackClassBinding = status === "INVALID";
    });

    this.control.valueChanges.subscribe(() => {
      this.invalidFeedbackClassBinding = this.control.touched && this.control.invalid;
    });

    this.invalidFeedbackClassBinding = this.control.invalid;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
