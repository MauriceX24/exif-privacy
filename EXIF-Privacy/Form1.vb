Imports System.Drawing.Imaging
Imports System.IO

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button2.Enabled = False
        Label1.Text = $"Status: nothing."
    End Sub
    Dim choosenPic As String
    Dim dragNDropPic As String
    Dim choosenFormat As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox1.Items.Clear()
        OpenFileDialog1.Filter = "PNG|*.png|jpeg|*.jpeg|JPG|*.jpg"
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Dim filePic As String = OpenFileDialog1.FileName
            choosenPic = OpenFileDialog1.FileName
            choosenFormat = OpenFileDialog1.FileName.Split(".")(1)
            Dim image As Image = Image.FromFile(filePic)
            PictureBox1.Image = image
            Dim propertyItems As PropertyItem() = image.PropertyItems

            For Each item As PropertyItem In propertyItems
                'Read Items
                ListBox1.Items.Add(item.Id & ": " & System.Text.Encoding.ASCII.GetString(item.Value))
            Next
            Button2.Enabled = True
            Label1.Text = $"Status: Ready to hide your datas"
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim image As Image = Image.FromFile(choosenPic)
        Dim propertyItems As PropertyItem() = image.PropertyItems
        Dim counter As Integer = 0
        For Each item As PropertyItem In propertyItems
            'Change Items
            If item.Id = 33432 Then 'EXIF-Copyright'
                item.Value = System.Text.Encoding.UTF8.GetBytes("Copyright © Lordmaurice.xyz")
                image.SetPropertyItem(item)
            Else
                item.Value = System.Text.Encoding.ASCII.GetBytes(" ")
                image.SetPropertyItem(item)
            End If
            counter += 1
        Next

        If choosenPic.Contains(".jpeg") Or choosenPic.Contains(".jpg") Or choosenPic.Contains(".png") Then
            Dim s As String
            s = choosenPic.Split(".")(0).Replace("png", "")
            s = choosenPic.Split(".")(0).Replace("jpg", "")
            s = choosenPic.Split(".")(0).Replace("jpeg", "")
            image.Save($"{s}({TimeOfDay.Hour & TimeOfDay.Minute & TimeOfDay.Second}) -PRIVACY.{choosenFormat}")
            checkAfterChange($"{s}({TimeOfDay.Hour & TimeOfDay.Minute & TimeOfDay.Second}) -PRIVACY.{choosenFormat}")
        End If
        Button2.Enabled = False
        Label1.Text = $"Status: Datas cleared - {counter} items cleared"
    End Sub
    Private Function checkAfterChange(ByVal pic As String)
        ListBox1.Items.Clear()
        Dim image As Image = Image.FromFile(pic)
        Dim propertyItems As PropertyItem() = image.PropertyItems

        For Each item As PropertyItem In propertyItems
            'Hier können Sie die Daten aus dem Objekt extrahieren'
            ListBox1.Items.Add(item.Id & ": " & System.Text.Encoding.UTF8.GetString(item.Value))
        Next
    End Function
    Private Function dragAndDropRead()
        Try
            ListBox1.Items.Clear()
            Dim filePic As String = dragNDropPic
            choosenPic = dragNDropPic
            choosenFormat = dragNDropPic.Split(".")(1)
            Dim image As Image = Image.FromFile(filePic)
            PictureBox1.Image = image
            Dim propertyItems As PropertyItem() = image.PropertyItems

            For Each item As PropertyItem In propertyItems
                'Hier können Sie die Daten aus dem Objekt extrahieren'
                ListBox1.Items.Add(item.Id & ": " & System.Text.Encoding.ASCII.GetString(item.Value))
            Next
            Button2.Enabled = True
            Label1.Text = $"Status: Ready to hide your datas"
        Catch ex As Exception
            Label1.Text = "Status: Could not read this"
        End Try
    End Function
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        Try
            Dim filePaths As String() = e.Data.GetData(DataFormats.FileDrop)
            Dim filePath As String = filePaths(0)
            Dim pic As Image = Image.FromFile(filePath)
            PictureBox1.Image = pic
            dragNDropPic = filePath
            dragAndDropRead()
        Catch ex As Exception
            Label1.Text = "Status: Please choose another file"
        End Try
    End Sub
End Class
