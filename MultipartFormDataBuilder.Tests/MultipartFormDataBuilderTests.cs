using FluentAssertions;
using MultipartFormDataBuilder;
using MultipartFormDataBuilder.Tests.Enums;
using MultipartFormDataBuilder.Tests.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MultipartFormDataBuilder.Tests
{
    public class MultipartFormDataBuilderTests
    {
        [Fact]
        public async void Should_GetTypePropertiesAsKeyValuePairs()
        {
            var vm = new BasicViewModel
            {
                Id = 1,
                Title = "VM 1",
                IsValid = true,
                PrimaryColour = new SelectOptionViewModel<Colour>
                {
                    Id = Colour.Red,
                    Name = "Red"
                },
                AdditionalColours = new List<SelectOptionViewModel<Colour>>
                {
                    new SelectOptionViewModel<Colour>
                    {
                        Id = Colour.Red,
                        Name = "Red"
                    }
                }
            };

            var formData = new MultipartFormDataContent();
            formData.AddStringContent(nameof(vm.Id), vm.Id);
            formData.AddStringContent(nameof(vm.Title), vm.Title);
            formData.AddStringContent(nameof(vm.IsValid), vm.IsValid);
            formData.AddTypeProperties(vm.PrimaryColour, nameof(vm.PrimaryColour));
            formData.AddTypeCollectionProperties(vm.AdditionalColours, nameof(vm.AdditionalColours));
            
            var formContent = formData.ToArray();

            var idFormContent = await formContent.ElementAt(0).ReadAsStringAsync();
            idFormContent.Should().Be(vm.Id.ToString());

            var titleFormContent = await formContent.ElementAt(1).ReadAsStringAsync();
            titleFormContent.Should().Be(vm.Title.ToString());

            var isValidFormContent = await formContent.ElementAt(2).ReadAsStringAsync();
            isValidFormContent.Should().Be(vm.IsValid.ToString());

            var primaryColourIdFormContent = await formContent.ElementAt(3).ReadAsStringAsync();
            primaryColourIdFormContent.Should().Be(((int)vm.PrimaryColour.Id).ToString());

            var primaryColourNameFormContent = await formContent.ElementAt(4).ReadAsStringAsync();
            primaryColourNameFormContent.Should().Be(vm.PrimaryColour.Name.ToString());

            var additionalColours1IdFormContent = await formContent.ElementAt(5).ReadAsStringAsync();
            additionalColours1IdFormContent.Should().Be(((int)vm.AdditionalColours.First().Id).ToString());

            var additionalColours1NameFormContent = await formContent.ElementAt(6).ReadAsStringAsync();
            additionalColours1NameFormContent.Should().Be(vm.AdditionalColours.First().Name.ToString());
        }
    }
}
