
export interface StudentProfile {
  studentId: number;
  name: string;
  email: string;
  department: string;
  age: number;
  cgpa: number;
  status: number;
  registeredAt: string;
  reviewedAt: string | null;
}

export interface UpdateStudentProfileRequest {
  name: string;
  department: string;
  age: number;
  cgpa: number;
}
