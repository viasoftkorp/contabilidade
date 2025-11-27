import { ValidatorFn, AbstractControl } from "@angular/forms";

export function atLeastOneTrue(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any; } | null => {
        const formArray = control.value as Array<any>;
        const hasAtLeastOneTrue = formArray.some(item => item === true);
        return hasAtLeastOneTrue ? null : { atLeastOneTrue: true };
    };
}