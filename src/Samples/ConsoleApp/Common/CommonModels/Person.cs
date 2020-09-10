using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModels
{
  public class Person
  {
    /// <summary>
    /// Gets or sets the PersonRole
    /// </summary>
    public string PersonRole { get; set; }

    /// <summary>
    /// Gets or sets the LastName
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the FirstName
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the MiddleName
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Gets the FullName
    /// </summary>
    public string FullName { get => GetFullName(); }

    /// <summary>
    /// Gets the LastNameFirst
    /// </summary>
    public string LastNameFirst { get => GetLastNameFirst(); }

    /// <summary>
    /// The GetLastNameFirst
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    private string GetLastNameFirst()
    {
      if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrEmpty(LastName))
      {
        return $"{LastName}, {FirstName}";
      }
      else if (!string.IsNullOrWhiteSpace(FirstName))
      {
        return $"{FirstName}";
      }
      else if (!string.IsNullOrWhiteSpace(LastName))
      {
        return $"{LastName}";
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// The GetFullName
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    private string GetFullName()
    {
      if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrEmpty(LastName))
      {
        return $"{FirstName} {LastName}";
      }
      else if (!string.IsNullOrWhiteSpace(FirstName))
      {
        return $"{FirstName}";
      }
      else if (!string.IsNullOrWhiteSpace(LastName))
      {
        return $"{LastName}";
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Gets or sets the PersonReligion
    /// </summary>
    public string PersonReligion { get; set; }

    /// <summary>
    /// Gets or sets the Login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Gets or sets the PersonStatus
    /// </summary>
    public string PersonStatus { get; set; }

    /// <summary>
    /// Gets or sets the PersonSex
    /// </summary>
    public string PersonSex { get; set; }

    /// <summary>
    /// Gets or sets the PersonRace
    /// </summary>
    public string PersonRace { get; set; }

    /// <summary>
    /// Gets or sets the PersonEthnicity
    /// </summary>
    public string PersonEthnicity { get; set; }

    /// <summary>
    /// Gets or sets the PersonEyeColor
    /// </summary>
    public string PersonEyeColor { get; set; }

    /// <summary>
    /// Gets or sets the PersonBuild
    /// </summary>
    public string PersonBuild { get; set; }

    /// <summary>
    /// Gets or sets the PersonHairColor
    /// </summary>
    public string PersonHairColor { get; set; }

    /// <summary>
    /// Gets or sets the PersonSkinTone
    /// </summary>
    public string PersonSkinTone { get; set; }

    /// <summary>
    /// Gets or sets the PersonHairStyle
    /// </summary>
    public string PersonHairStyle { get; set; }

    /// <summary>
    /// Gets or sets the PersonHairType
    /// </summary>
    public string PersonHairType { get; set; }

    /// <summary>
    /// Gets or sets the PersonFacialHair
    /// </summary>
    public string PersonFacialHair { get; set; }

    /// <summary>
    /// Gets or sets the PersonTeeth
    /// </summary>
    public string PersonTeeth { get; set; }

    /// <summary>
    /// Gets or sets the PersonComplexion
    /// </summary>
    public string PersonComplexion { get; set; }

    /// <summary>
    /// Gets or sets the PersonMaritalStatus
    /// </summary>
    public string PersonMaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the PersonSexualOrientation
    /// </summary>
    public string PersonSexualOrientation { get; set; }

    /// <summary>
    /// Gets or sets the PersonDrivingInsCoverageCategory
    /// </summary>
    public string PersonDrivingInsCoverageCategory { get; set; }

    /// <summary>
    /// Gets or sets the PersonDrivingInsuranceStatus
    /// </summary>
    public string PersonDrivingInsuranceStatus { get; set; }

    /// <summary>
    /// Gets or sets the PersonalHairAppearance
    /// </summary>
    public string PersonalHairAppearance { get; set; }

    /// <summary>
    /// Gets or sets the PersonJewelry
    /// </summary>
    public string PersonJewelry { get; set; }

    /// <summary>
    /// Gets or sets the PersonGeneralAppearance
    /// </summary>
    public string PersonGeneralAppearance { get; set; }

    /// <summary>
    /// Gets or sets the PersonDisguise
    /// </summary>
    public string PersonDisguise { get; set; }

    /// <summary>
    /// Gets or sets the PersonHandedness
    /// </summary>
    public string PersonHandedness { get; set; }

    /// <summary>
    /// Gets or sets the PersonSpeech
    /// </summary>
    public string PersonSpeech { get; set; }

    /// <summary>
    /// Gets or sets the SsnIdentification
    /// </summary>
    public string SsnIdentification { get; set; }

    /// <summary>
    /// Gets or sets the PersonBirthDate
    /// </summary>
    public DateTime? PersonBirthDate { get; set; }

    /// <summary>
    /// Gets or sets the PersonBirthCity
    /// </summary>
    public string PersonBirthCity { get; set; }

    /// <summary>
    /// Gets or sets the PersonBirthState
    /// </summary>
    public string PersonBirthState { get; set; }

    /// <summary>
    /// Gets or sets the PersonOccupation
    /// </summary>
    public string PersonOccupation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether PersonCitizenStatus
    /// </summary>
    public bool? PersonCitizenStatus { get; set; }

    /// <summary>
    /// Gets or sets the PersonDeathDate
    /// </summary>
    public DateTime? PersonDeathDate { get; set; }

    /// <summary>
    /// Gets or sets the PersonHeight
    /// </summary>
    public double? PersonHeight { get; set; }

    /// <summary>
    /// Gets or sets the PersonHeightMin
    /// </summary>
    public double? PersonHeightMin { get; set; }

    /// <summary>
    /// Gets or sets the PersonHeightmax
    /// </summary>
    public double? PersonHeightmax { get; set; }

    /// <summary>
    /// Gets or sets the PersonWeight
    /// </summary>
    public double? PersonWeight { get; set; }

    /// <summary>
    /// Gets or sets the PersonWeightMin
    /// </summary>
    public double? PersonWeightMin { get; set; }

    /// <summary>
    /// Gets or sets the PersonWeightMax
    /// </summary>
    public double? PersonWeightMax { get; set; }

    /// <summary>
    /// Gets or sets the FbiIdentification
    /// </summary>
    public string FbiIdentification { get; set; }

    /// <summary>
    /// Gets or sets the StateFingerprintIdentification
    /// </summary>
    public string StateFingerprintIdentification { get; set; }

    /// <summary>
    /// Gets or sets the TaxIdentification
    /// </summary>
    public string TaxIdentification { get; set; }

    /// <summary>
    /// Gets or sets the PersonEyeWear
    /// </summary>
    public string PersonEyeWear { get; set; }

    /// <summary>
    /// Gets or sets the HomePhoneNumber
    /// </summary>
    public string HomePhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the WorkPhoneNumber
    /// </summary>
    public string WorkPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the Alias
    /// </summary>
    public string Alias { get; set; }
  }
}
