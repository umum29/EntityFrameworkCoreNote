using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineOrderApi.Models
{
/*
Here, I assume that Student and Grade tables are one-to-one relation.
I created some sample data for Student first, then, use WebApi to post(create) Grade record.
*/
  public class Student
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentId { get; set; }
    public string StudentName { get; set; }
  }
  public class Grade
  {
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }

    [ForeignKey("Student")]
    public int StudentId { get; set; }
    //must have ? to make it nullable, or WebApi will ask us to provide detail of Student when posting(creating) Grade data
    //if we did not set it as nullable(?), it will ask for details of Student data, and also create a new Student data, instead of referencing the existing student data
    //if we use ? here, in Grade table of database, it will only store StudentId(not creating a new Student record)
    //in GradeApiController, we can use "await _context.Grades.Include(s => s.Student).ToListAsync()" to show Grade/Student data together
    public Student? Student { get; set; }
  }
}
