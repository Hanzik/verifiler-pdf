using System;
using System.IO;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using NLog;
using VerifilerCore;

namespace VerifilerPDF {

	/// <summary>
	/// This validation step is using the iTextSharp library for
	/// manipulating PDF files.
	/// 
	/// The error code produced by this validation is Error.Corrupted.
	/// </summary>
	public class PDFValidator : FormatSpecificValidator {

		public override int ErrorCode { get; set; } = Error.Corrupted;

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public override void Setup() {
			Name = "PDF files verification";
			RelevantExtensions.Add(".pdf");
			Enable();
		}

		public override void ValidateFile(string file) {

			using (var existingFileStream = new FileStream(file, FileMode.Open)) {
				try {
					var pdfReader = new PdfReader(existingFileStream);
					pdfReader.Close();
				} catch (InvalidPdfException e) {
					ReportAsError("File is corrupted: " + file + "; Message: " + e.Message);
					GC.Collect();
				}
			}
		}
	}
}
