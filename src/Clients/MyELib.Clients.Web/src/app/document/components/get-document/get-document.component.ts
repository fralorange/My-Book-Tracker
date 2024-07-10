import { Component, OnInit } from '@angular/core';
import { DocumentService } from '../../services/document.service';
import { ActivatedRoute, Params } from '@angular/router';
import { DocumentModel } from '../../models/document.model';

@Component({
  selector: 'app-get-document',
  templateUrl: './get-document.component.html',
  styleUrl: './get-document.component.css'
})
export class GetDocumentComponent implements OnInit {
  private id: string = '';
  protected document!: DocumentModel

  constructor(private route: ActivatedRoute, private documentService: DocumentService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params["id"];
      this.getDocumentById();
    })
  }

  getDocumentById() {
    return this.documentService.getDocumentById(this.id)
      ?.subscribe((result: DocumentModel) => this.document = result);
  }

  get documentContentText() {
    return this.documentService.base64StringToText(this.document.content);
  }
}
