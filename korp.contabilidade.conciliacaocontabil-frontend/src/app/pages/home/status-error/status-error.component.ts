import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BuscarConciliacaoContabilOutput } from 'src/app/services/conciliacao-contabil/models/buscar-conciliacao-contabil-output.models';

@Component({
  selector: 'app-status-error',
  templateUrl: './status-error.component.html',
  styleUrl: './status-error.component.scss'
})
export class StatusErrorComponent {
  public copied = false;
  constructor(@Inject(MAT_DIALOG_DATA)
  public data: BuscarConciliacaoContabilOutput,
    private dialogRef: MatDialogRef<StatusErrorComponent>,
  ) { }

  closeModal() {
    this.dialogRef.close();
  }

  copyError() {
    navigator.clipboard.writeText(this.data.erro).then(() => {
      this.copied = true;
      setTimeout(() => {
        this.copied = false;
      }, 3000);
    }).catch(err => {
      console.error("Failed to copy: ", err);
    });
  }
}
