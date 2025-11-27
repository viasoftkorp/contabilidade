import { AbstractControl, ValidatorFn } from '@angular/forms';

export function endDateAfterStartDateValidator(startDateField: string, endDateField: string): ValidatorFn {
    return (formGroup: AbstractControl) => {
        const startDate = formGroup.get(startDateField)?.value;
        const endDate = formGroup.get(endDateField)?.value;

        if (!startDate || !endDate) {
            return null; // don't validate if either date is missing
        }

        return new Date(endDate) > new Date(startDate) ? null : { endDateBeforeStartDate: true };
    };
}
