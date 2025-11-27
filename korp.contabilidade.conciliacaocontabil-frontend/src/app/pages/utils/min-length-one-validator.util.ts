import { AbstractControl, ValidatorFn } from "@angular/forms";

export function atLeastOneItem(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any; } | null => {
        const formArray = control.value as Array<any>;
        return formArray.length > 0 ? null : { atLeastOneItem: true };
    };
}